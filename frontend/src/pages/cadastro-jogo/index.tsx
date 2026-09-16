import { useEffect, useState, useRef } from "react";
import { useRouter } from "next/router";
import Header from "@/components/header/header";
import Footer from "@/components/footer/footer";
import styles from "./cadastro-jogo.module.css";
import {
  cadastrarJogo,
  editarJogo,
  listarPorId,
  listarGeneros,
  Genero,
} from "@/services/jogoService";
import { listarPlataformas, Plataforma } from "@/services/plataformaService";
import {
  listarClassificacoes,
  Classificacao,
} from "@/services/classificacaoService";
import { verificarAutenticacao } from "@/utils/auth";
import { notificacao, erro } from "@/utils/toast";

export default function CadastroJogo() {
  const router = useRouter();
  const { id } = router.query;
  const isEdicao = Boolean(id);

  const fileInputRef = useRef<HTMLInputElement>(null);

  // Estados do formulário
  const [nome, setNome] = useState("");
  const [descricao, setDescricao] = useState("");
  const [preco, setPreco] = useState("");
  const [statusJogo, setStatusJogo] = useState(true);
  const [imagemArquivo, setImagemArquivo] = useState<File | null>(null);
  const [imagemPreview, setImagemPreview] = useState<string>("");

  // Seleções
  const [plataformasSelecionadas, setPlataformasSelecionadas] = useState<number[]>([]);
  const [generosSelecionados, setGenerosSelecionados] = useState<number[]>([]);
  const [classificacaoId, setClassificacaoId] = useState<number>(0);

  // Listas vindas do backend
  const [plataformas, setPlataformas] = useState<Plataforma[]>([]);
  const [generos, setGeneros] = useState<Genero[]>([]);
  const [classificacoes, setClassificacoes] = useState<Classificacao[]>([]);

  // Estados de controle
  const [carregando, setCarregando] = useState(false);
  const [salvando, setSalvando] = useState(false);

  // 1. Verificação de Autenticação
  useEffect(() => {
    if (!verificarAutenticacao()) {
      erro("Você precisa estar logado para cadastrar ou editar jogos!");
      router.push("/login");
    }
  }, []);

  // 2. Carregar listas auxiliares (Gêneros, Plataformas, Classificações)
  useEffect(() => {
    async function carregarAuxiliares() {
      try {
        const [listaGen, listaPlat, listaClass] = await Promise.all([
          listarGeneros(),
          listarPlataformas(),
          listarClassificacoes(),
        ]);
        setGeneros(listaGen);
        setPlataformas(listaPlat);
        setClassificacoes(listaClass);
      } catch (err: any) {
        console.error("Erro ao carregar dados auxiliares:", err);
      }
    }
    carregarAuxiliares();
  }, []);

  // 3. Se for modo edição, carregar dados do jogo
  useEffect(() => {
    if (!id) return;

    async function carregarJogoParaEdicao() {
      setCarregando(true);
      try {
        const jogo = await listarPorId(Number(id));
        setNome(jogo.nome);
        setDescricao(jogo.descricao);
        setPreco(jogo.preco.toString());
        setStatusJogo(jogo.statusJogo);
        setPlataformasSelecionadas(jogo.plataformaIds || []);
        setGenerosSelecionados(jogo.generoIds || []);
        if (jogo.classificacaoId) {
          setClassificacaoId(jogo.classificacaoId);
        }
        if (jogo.imagemUrl) {
          setImagemPreview(jogo.imagemUrl);
        }
      } catch (err: any) {
        erro(err.message || "Erro ao carregar dados do jogo");
        router.push("/home");
      } finally {
        setCarregando(false);
      }
    }

    carregarJogoParaEdicao();
  }, [id]);

  // Manipulação de imagem
  function handleImageChange(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (file) {
      setImagemArquivo(file);
      setImagemPreview(URL.createObjectURL(file));
    }
  }

  // Toggle de seleção múltipla (Plataforma)
  function togglePlataforma(platId: number) {
    setPlataformasSelecionadas((prev) =>
      prev.includes(platId) ? prev.filter((item) => item !== platId) : [...prev, platId]
    );
  }

  // Toggle de seleção múltipla (Gênero)
  function toggleGenero(genId: number) {
    setGenerosSelecionados((prev) =>
      prev.includes(genId) ? prev.filter((item) => item !== genId) : [...prev, genId]
    );
  }

  // Submissão do Formulário
  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    if (!nome.trim()) {
      erro("O nome do jogo é obrigatório!");
      return;
    }
    if (!descricao.trim()) {
      erro("A descrição do jogo é obrigatória!");
      return;
    }
    const precoNum = parseFloat(preco.toString().replace(",", "."));
    if (isNaN(precoNum) || precoNum <= 0) {
      erro("Informe um preço válido maior que zero!");
      return;
    }
    if (!isEdicao && !imagemArquivo) {
      erro("Selecione uma imagem para o jogo!");
      return;
    }
    if (plataformasSelecionadas.length === 0) {
      erro("Selecione pelo menos uma plataforma!");
      return;
    }
    if (generosSelecionados.length === 0) {
      erro("Selecione pelo menos um gênero!");
      return;
    }
    if (!classificacaoId) {
      erro("Selecione a classificação indicativa!");
      return;
    }

    setSalvando(true);
    try {
      const payload = {
        nome,
        descricao,
        preco: precoNum,
        statusJogo,
        imagem: imagemArquivo,
        plataformaIds: plataformasSelecionadas,
        generoIds: generosSelecionados,
        classificacaoId,
      };

      if (isEdicao) {
        await editarJogo(Number(id), payload);
        notificacao("Jogo atualizado com sucesso!");
      } else {
        await cadastrarJogo(payload);
        notificacao("Jogo cadastrado com sucesso!");
      }

      setTimeout(() => {
        router.push("/home");
      }, 1500);
    } catch (err: any) {
      erro(err.message || "Erro ao salvar o jogo");
    } finally {
      setSalvando(false);
    }
  }

  return (
    <div className={styles.container}>
      <Header />
      <main className={styles.conteudo}>
        <div className={styles.cabecalho}>
          <h1 className={styles.titulo}>
            {isEdicao ? "Editar Jogo" : "Cadastrar Novo Jogo"}
          </h1>
          <p className={styles.subtitulo}>
            {isEdicao
              ? "Atualize as informações do título selecionado"
              : "Preencha as informações para adicionar um novo jogo ao catálogo"}
          </p>
          <div className={styles.linha_decorativa}></div>
        </div>

        {carregando ? (
          <p style={{ textAlign: "center", color: "#a09cb0" }}>Carregando dados...</p>
        ) : (
          <form className={styles.formulario} onSubmit={handleSubmit}>
            {/* Nome do Jogo */}
            <div className={styles.campo}>
              <label htmlFor="nome">Título do Jogo</label>
              <input
                id="nome"
                type="text"
                value={nome}
                onChange={(e) => setNome(e.target.value)}
                placeholder="Ex: Cyberpunk 2077, Dying Light..."
                required
              />
            </div>

            {/* Preço e Classificação Indicativa */}
            <div className={styles.linhaDupla}>
              <div className={styles.campo}>
                <label htmlFor="preco">Preço (R$)</label>
                <input
                  id="preco"
                  type="number"
                  step="0.01"
                  min="0.01"
                  value={preco}
                  onChange={(e) => setPreco(e.target.value)}
                  placeholder="Ex: 199.90"
                  required
                />
              </div>

              <div className={styles.campo}>
                <label htmlFor="classificacao">Classificação Indicativa</label>
                <select
                  id="classificacao"
                  value={classificacaoId}
                  onChange={(e) => setClassificacaoId(Number(e.target.value))}
                  required
                >
                  <option value={0}>Selecione uma classificação</option>
                  {classificacoes.map((item) => (
                    <option key={item.classificacaoId} value={item.classificacaoId}>
                      {item.nomeClassificacao}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            {/* Descrição */}
            <div className={styles.campo}>
              <label htmlFor="descricao">Descrição Completa</label>
              <textarea
                id="descricao"
                value={descricao}
                onChange={(e) => setDescricao(e.target.value)}
                placeholder="Detalhes, história, características principais do jogo..."
                required
              />
            </div>

            {/* Upload de Imagem */}
            <div className={styles.campo}>
              <label>Imagem da Capa {isEdicao ? "(Opcional para manter a atual)" : ""}</label>
              <input
                type="file"
                ref={fileInputRef}
                style={{ display: "none" }}
                accept="image/*"
                onChange={handleImageChange}
              />
              <div
                className={styles.uploadBox}
                onClick={() => fileInputRef.current?.click()}
              >
                <span style={{ fontSize: "1.8rem" }}>📁</span>
                <span>
                  {imagemArquivo
                    ? `Arquivo: ${imagemArquivo.name}`
                    : "Clique aqui para selecionar uma imagem do computador"}
                </span>
                <small style={{ color: "#a09cb0" }}>Formatos suportados: JPG, PNG, WEBP</small>
              </div>

              {imagemPreview && (
                <div className={styles.previewContainer}>
                  <img
                    src={imagemPreview}
                    alt="Pré-visualização"
                    className={styles.imagemPreview}
                  />
                </div>
              )}
            </div>

            {/* Gêneros */}
            <div className={styles.campo}>
              <label>Gêneros (Selecione ao menos um)</label>
              <div className={styles.chipsGrid}>
                {generos.map((gen) => {
                  const selecionado = generosSelecionados.includes(gen.generoId);
                  return (
                    <div
                      key={gen.generoId}
                      className={`${styles.chipItem} ${selecionado ? styles.chipAtivo : ""}`}
                      onClick={() => toggleGenero(gen.generoId)}
                    >
                      <span>{selecionado ? "✓" : "+"}</span>
                      <span>{gen.nome}</span>
                    </div>
                  );
                })}
              </div>
            </div>

            {/* Plataformas */}
            <div className={styles.campo}>
              <label>Plataformas Disponíveis</label>
              <div className={styles.chipsGrid}>
                {plataformas.map((plat) => {
                  const selecionada = plataformasSelecionadas.includes(plat.plataformaId);
                  return (
                    <div
                      key={plat.plataformaId}
                      className={`${styles.chipItem} ${selecionada ? styles.chipAtivo : ""}`}
                      onClick={() => togglePlataforma(plat.plataformaId)}
                    >
                      <span>{selecionada ? "✓" : "+"}</span>
                      <span>{plat.nome}</span>
                    </div>
                  );
                })}
              </div>
            </div>

            {/* Status do Jogo */}
            <div className={styles.campo}>
              <label>Status do Jogo no Catálogo</label>
              <div className={styles.statusToggle}>
                <label className={styles.switch}>
                  <input
                    type="checkbox"
                    checked={statusJogo}
                    onChange={(e) => setStatusJogo(e.target.checked)}
                  />
                  <span className={styles.slider}></span>
                </label>
                <span style={{ color: statusJogo ? "#00e676" : "#ff5252", fontWeight: "bold" }}>
                  {statusJogo ? "Ativo (Visível para compra)" : "Inativo (Oculto)"}
                </span>
              </div>
            </div>

            {/* Botões de Ação */}
            <div className={styles.acoes}>
              <button
                type="button"
                className={styles.btnCancelar}
                onClick={() => router.push("/home")}
                disabled={salvando}
              >
                Cancelar
              </button>
              <button type="submit" className={styles.btnSalvar} disabled={salvando}>
                {salvando
                  ? "Salvando..."
                  : isEdicao
                  ? "Atualizar Jogo"
                  : "Cadastrar Jogo"}
              </button>
            </div>
          </form>
        )}
      </main>
      <Footer />
    </div>
  );
}

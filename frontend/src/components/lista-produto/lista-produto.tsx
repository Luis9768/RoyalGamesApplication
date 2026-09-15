import styles from "@/components/lista-produto/lista-produto.module.css";
import CardProduto from "../card-produto/card-produto";
import Paginacao from "@/components/paginacao/paginacao";
import { useEffect, useState } from "react";
import { excluirJogo, listarJogo, listarGeneros, Jogo, Genero } from "@/services/jogoService";
import { notificacao, toastConfirmarExclusao, erro } from "@/utils/toast";
import { verificarAutenticacao } from "@/utils/auth";

const ListaProduto = () => {
  const [jogos, setJogos] = useState<Jogo[]>([]);
  const [listaGeneros, setListaGeneros] = useState<Genero[]>([]);

  const [ordem, setOrdem] = useState("todos");
  const [pesquisa, setPesquisa] = useState("");
  const [estaAutenticado, setEstaAutenticado] = useState(false);

  const [genero, setGenero] = useState("todos");
  const [paginaAtual, setPaginaAtual] = useState(1);
  const itensPorPagina = 6;

  async function listar() {
    try {
      const lista = await listarJogo();
      setJogos(lista);
    } catch (error: any) {
      console.error("Erro ao listar jogos:", error);
    }
  }

  async function buscarGeneros() {
    try {
      const generosDoBanco = await listarGeneros();
      setListaGeneros(generosDoBanco);
    } catch (error: any) {
      console.error("Erro ao buscar gêneros:", error);
    }
  }

  function confirmarExclusao(jogoId: number) {
    toastConfirmarExclusao(async () => {
      try {
        await excluirJogo(jogoId);
        setJogos((listaAtual) =>
          listaAtual.map((j) =>
            j.jogoId === jogoId ? { ...j, statusJogo: false } : j
          )
        );
        notificacao("Jogo inativado com sucesso!");
        listar();
      } catch (error: any) {
        erro(error.message || "Erro ao inativar jogo");
      }
    });
  }

  useEffect(() => {
    setEstaAutenticado(verificarAutenticacao());
    listar();
    buscarGeneros();
  }, []);

  useEffect(() => {
    setPaginaAtual(1);
  }, [pesquisa, ordem, genero]);

  // Filtra por ativos (se não estiver logado como admin) e pesquisa
  let jogosProcessados = jogos.filter((jogo) =>
    estaAutenticado ? true : jogo.statusJogo === true
  );

  if (pesquisa.trim() !== "") {
    jogosProcessados = jogosProcessados.filter((jogo) =>
      jogo.nome.toLowerCase().includes(pesquisa.toLowerCase())
    );
  }

  if (genero !== "todos") {
    jogosProcessados = jogosProcessados.filter((jogo) =>
      jogo.generos?.some((g) => g.toLowerCase() === genero.toLowerCase())
    );
  }

  jogosProcessados.sort((a, b) => {
    if (ordem === "menor_valor") {
      return a.preco - b.preco;
    } else if (ordem === "maior_valor") {
      return b.preco - a.preco;
    }
    return a.jogoId - b.jogoId;
  });

  const totalPaginas = Math.ceil(jogosProcessados.length / itensPorPagina);
  const indexUltimoItem = paginaAtual * itensPorPagina;
  const indexPrimeiroItem = indexUltimoItem - itensPorPagina;

  const jogosPaginados = jogosProcessados.slice(
    indexPrimeiroItem,
    indexUltimoItem
  );

  const lidarComMudancaPagina = (
    _event: React.ChangeEvent<unknown>,
    value: number
  ) => {
    setPaginaAtual(value);
  };

  return (
    <>
      <h2>Catálogo de jogos</h2>

      <div id={styles.filtros}>
        <div className={styles.campoPesquisa}>
          <input
            type="text"
            name="pesquisa"
            placeholder="Pesquisar por título..."
            value={pesquisa}
            onChange={(e) => setPesquisa(e.target.value)}
          />
        </div>

        <select
          className={styles.botao}
          value={ordem}
          onChange={(e) => setOrdem(e.target.value)}
        >
          <option value="todos">Ordenar por preço</option>
          <option value="menor_valor">Menor valor</option>
          <option value="maior_valor">Maior valor</option>
        </select>

        {/* Select de Gêneros */}
        <select
          className={styles.botao}
          value={genero}
          onChange={(e) => setGenero(e.target.value)}
        >
          <option value="todos">Todos os gêneros</option>
          {listaGeneros.map((gen) => (
            <option key={gen.generoId} value={gen.nome}>
              {gen.nome}
            </option>
          ))}
        </select>
      </div>

      <div className={styles.gridProdutos}>
        {jogosPaginados.length > 0 ? (
          jogosPaginados.map((item) => (
            <CardProduto
              key={item.jogoId}
              jogoId={item.jogoId}
              titulo={item.nome}
              descricao={item.descricao}
              preco={item.preco}
              img={item.imagemUrl}
              autenticado={estaAutenticado}
              onExcluir={confirmarExclusao}
            />
          ))
        ) : (
          <p style={{ gridColumn: "1 / -1", textAlign: "center", color: "#a09cb0" }}>
            Nenhum jogo encontrado com os filtros selecionados.
          </p>
        )}
      </div>

      {totalPaginas > 1 && (
        <Paginacao
          count={totalPaginas}
          page={paginaAtual}
          onChange={lidarComMudancaPagina}
        />
      )}
    </>
  );
};

export default ListaProduto;
import { useEffect, useState } from "react";
import { useRouter } from "next/router";
import Link from "next/link";
import Header from "@/components/header/header";
import Footer from "@/components/footer/footer";
import styles from "./detalhe-produto.module.css";
import { listarPorId, Jogo } from "@/services/jogoService";
import { formatarPreco } from "@/utils/formatacao";
import { verificarAutenticacao } from "@/utils/auth";

export default function DetalheProduto() {
  const router = useRouter();
  const { id } = router.query;

  const [jogo, setJogo] = useState<Jogo | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [erroMsg, setErroMsg] = useState("");
  const [autenticado, setAutenticado] = useState(false);

  useEffect(() => {
    setAutenticado(verificarAutenticacao());
  }, []);

  useEffect(() => {
    if (!router.isReady || !id) return;

    async function buscarJogo() {
      setCarregando(true);
      setErroMsg("");
      try {
        const dados = await listarPorId(Number(id));
        setJogo(dados);
      } catch (err: any) {
        setErroMsg(err.message || "Não foi possível carregar os detalhes deste jogo.");
      } finally {
        setCarregando(false);
      }
    }

    buscarJogo();
  }, [router.isReady, id]);

  return (
    <div className={styles.container}>
      <Header />
      <main className={styles.conteudo}>
        <div className={styles.barraNavegacao}>
          <button className={styles.btnVoltar} onClick={() => router.push("/home")}>
            ← Voltar ao Catálogo
          </button>

          {autenticado && jogo && (
            <Link href={`/cadastro-jogo?id=${jogo.jogoId}`}>
              <button className={styles.btnEditar}>✏️ Editar este Jogo</button>
            </Link>
          )}
        </div>

        {carregando ? (
          <div className={styles.mensagemCarregando}>
            <p>Carregando informações do jogo...</p>
          </div>
        ) : erroMsg || !jogo ? (
          <div className={styles.mensagemErro}>
            <p>{erroMsg || "Jogo não encontrado!"}</p>
            <button
              className={styles.btnVoltar}
              style={{ margin: "20px auto 0 auto" }}
              onClick={() => router.push("/home")}
            >
              Voltar à página inicial
            </button>
          </div>
        ) : (
          <article className={styles.detalheCard}>
            {/* Coluna da Imagem */}
            <div className={styles.colunaImagem}>
              <img
                src={jogo.imagemUrl}
                alt={`Capa do jogo ${jogo.nome}`}
                className={styles.imagemCapa}
                onError={(e) => {
                  (e.target as HTMLImageElement).src = "/imgs/banner.png";
                }}
              />
            </div>

            {/* Coluna de Informações */}
            <div className={styles.colunaInfo}>
              <div className={styles.cabecalhoInfo}>
                <h1 className={styles.titulo}>{jogo.nome}</h1>

                <div className={styles.blocoPreco}>
                  <span className={styles.preco}>{formatarPreco(jogo.preco)}</span>
                  <span
                    className={`${styles.statusTag} ${
                      jogo.statusJogo ? styles.statusAtivo : styles.statusInativo
                    }`}
                  >
                    {jogo.statusJogo ? "Disponível" : "Indisponível"}
                  </span>
                </div>

                <div className={styles.descricao}>
                  <p>{jogo.descricao}</p>
                </div>
              </div>

              {/* Metadados: Classificação, Plataformas e Gêneros */}
              <div className={styles.metadadosGrid}>
                <div className={styles.metaItem}>
                  <span className={styles.metaTitulo}>Classificação Indicativa</span>
                  <div className={styles.badgesContainer}>
                    <span className={`${styles.badge} ${styles.badgeDestaque}`}>
                      {jogo.classificacao || "Não informada"}
                    </span>
                  </div>
                </div>

                <div className={styles.metaItem}>
                  <span className={styles.metaTitulo}>Plataformas</span>
                  <div className={styles.badgesContainer}>
                    {jogo.plataformas && jogo.plataformas.length > 0 ? (
                      jogo.plataformas.map((plat, index) => (
                        <span key={index} className={styles.badge}>
                          {plat}
                        </span>
                      ))
                    ) : (
                      <span className={styles.badge}>PC</span>
                    )}
                  </div>
                </div>

                <div className={styles.metaItem} style={{ gridColumn: "1 / -1" }}>
                  <span className={styles.metaTitulo}>Gêneros</span>
                  <div className={styles.badgesContainer}>
                    {jogo.generos && jogo.generos.length > 0 ? (
                      jogo.generos.map((gen, index) => (
                        <span key={index} className={`${styles.badge} ${styles.badgeDestaque}`}>
                          {gen}
                        </span>
                      ))
                    ) : (
                      <span className={styles.badge}>Geral</span>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </article>
        )}
      </main>
      <Footer />
    </div>
  );
}
import CardProduto from "@/components/card-produto/card-produto";
import Footer from "@/components/footer/footer";
import Header from "@/components/header/header";
import InfoBanner from "@/components/info-banner/info-banner";
import ListaProduto from "@/components/lista-produto/lista-produto";
import styles from "@/pages/home/home.module.css";
import Link from "next/link";

const Home = () => {
  return (
    <>
      <Header />
      <main>
        <section className={styles.banner}>
          <div className={`${styles.container_banner} layout_guide`}>
            <div className={styles.textos_banner}>
              <h1 className={styles.titulo_banner}>Conheça nossos jogos!</h1>
              <h2 className={styles.subtitulo_banner}>
                Navegue por títulos de todas as gerações, descubra plataformas,
                gêneros e detalhes completos antes de escolher sua próxima
                aventura. Seu próximo jogo favorito começa aqui.
              </h2>
            </div>
            <img
              src="/imgs/banner.png"
              alt="Foto futurista de uma mulher no estilo cyberpunk"
            />
          </div>
        </section>
        <section id={styles.catalogo}>
          <ListaProduto />
        </section>
        <section>
          <InfoBanner />
        </section>
      </main>
      <Footer />
    </>
  );
};
export default Home;

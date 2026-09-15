import Header from "@/components/header/header";
import Footer from "@/components/footer/footer";
import ListaProduto from "@/components/lista-produto/lista-produto";
import styles from "@/pages/home/home.module.css";

export default function CatalogoPage() {
  return (
    <>
      <Header />
      <main style={{ minHeight: "80vh", backgroundColor: "var(--fundo-roxo-escuro, #221c2d)" }}>
        <section id={styles.catalogo} style={{ paddingTop: "60px", paddingBottom: "80px" }}>
          <ListaProduto />
        </section>
      </main>
      <Footer />
    </>
  );
}

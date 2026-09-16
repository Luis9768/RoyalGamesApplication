import styles from "@/components/footer/footer.module.css";

const Footer = () => {
  return (
    <footer className={styles.rodape}>
      <div className={styles.rodape__conteudo}>
        
        <div className={styles.rodape__logo}>
          <img 
            src="/imgs/logo_royal.png" 
            alt="Logo Royal Games" 
            className={styles.rodape__logoImagem}
          />
        </div>

        <div className={styles.rodape__academico}>
          <p className={styles.rodape__avisoTitulo}>🎓 Projeto Acadêmico Sem Fins Lucrativos</p>
          <p className={styles.rodape__avisoTexto}>
            Este website foi desenvolvido exclusivamente para fins de estudo, portfólio e aprendizado acadêmico.
            Não possui finalidade comercial. Todas as marcas, imagens e títulos pertencem aos seus respectivos proprietários.
          </p>
        </div>

        <ul className={styles.rodape__contatos}>
          <li className={styles.rodape__contatoItem}>royalgames@email.com</li>
          <li className={styles.rodape__contatoItem}>(11) 99999-9999</li>
          <li className={styles.rodape__contatoItem}>@RoyalGames</li>
        </ul>

      </div>

      <div className={styles.rodape__copyright}>
        <p>© {new Date().getFullYear()} Royal Games — Desenvolvido para fins educacionais e demonstração técnica.</p>
      </div>
    </footer>
  );
};

export default Footer;
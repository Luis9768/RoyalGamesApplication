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

        <ul className={styles.rodape__contatos}>
          <li className={styles.rodape__contatoItem}>royalgames@email.com</li>
          <li className={styles.rodape__contatoItem}>(11)99999-9999</li>
          <li className={styles.rodape__contatoItem}>@RoyalGames</li>
        </ul>

      </div>
    </footer>
  )
}

export default Footer;
import { useEffect, useState } from "react";
import Link from "next/link";
import { useRouter } from "next/router";
import styles from "./header.module.css";
import { verificarAutenticacao } from "@/utils/auth";
import { logout } from "@/services/authService";
import { notificacao } from "@/utils/toast";

const Header = () => {
  const router = useRouter();
  const [autenticado, setAutenticado] = useState(false);
  const [menuAberto, setMenuAberto] = useState(false);

  useEffect(() => {
    setAutenticado(verificarAutenticacao());
  }, []);

  // Fechar o menu mobile sempre que navegar de rota
  useEffect(() => {
    setMenuAberto(false);
  }, [router.asPath]);

  function handleLogout() {
    logout();
    setAutenticado(false);
    setMenuAberto(false);
    notificacao("Você saiu da sua conta.");
    router.push("/home");
  }

  return (
    <header className={styles.header}>
      <div className={styles.container}>
        <Link href="/home" className={styles.logo_link}>
          <img
            src="/imgs/logo_royal.png"
            alt="Logo Royal Games"
            className={styles.logo_imagem}
          />
        </Link>

        {/* Navegação Desktop */}
        <nav className={styles.nav_desktop}>
          <Link href="/catalogo">
            <button className={styles.botao_catalogo}>Catálogo</button>
          </Link>

          {autenticado ? (
            <>
              <Link href="/cadastro-jogo">
                <button className={styles.botao_novo}>+ Novo Jogo</button>
              </Link>
              <button className={styles.botao_logout} onClick={handleLogout}>
                Sair
              </button>
            </>
          ) : (
            <Link href="/login">
              <button className={styles.botao_login}>Login</button>
            </Link>
          )}
        </nav>

        {/* Botão Hambúrguer Mobile */}
        <button
          className={`${styles.btn_hamburguer} ${menuAberto ? styles.hamburguer_aberto : ""}`}
          onClick={() => setMenuAberto(!menuAberto)}
          aria-label={menuAberto ? "Fechar menu" : "Abrir menu"}
          aria-expanded={menuAberto}
        >
          <span className={`${styles.linha_hamburguer} ${styles.linha1}`}></span>
          <span className={`${styles.linha_hamburguer} ${styles.linha2}`}></span>
          <span className={`${styles.linha_hamburguer} ${styles.linha3}`}></span>
        </button>
      </div>

      {/* Drawer Menu Mobile */}
      {menuAberto && (
        <div className={`${styles.menu_mobile} ${styles.menu_mobile_aberto}`}>
          <Link href="/catalogo" className={styles.item_mobile} onClick={() => setMenuAberto(false)}>
            <button className={`${styles.botao_catalogo} ${styles.btn_mobile_item}`}>
              Catálogo
            </button>
          </Link>

          {autenticado ? (
            <>
              <Link href="/cadastro-jogo" className={styles.item_mobile} onClick={() => setMenuAberto(false)}>
                <button className={`${styles.botao_novo} ${styles.btn_mobile_item}`}>
                  + Novo Jogo
                </button>
              </Link>
              <button
                className={`${styles.botao_logout} ${styles.btn_mobile_item}`}
                onClick={handleLogout}
              >
                Sair
              </button>
            </>
          ) : (
            <Link href="/login" className={styles.item_mobile} onClick={() => setMenuAberto(false)}>
              <button className={`${styles.botao_login} ${styles.btn_mobile_item}`}>
                Login
              </button>
            </Link>
          )}
        </div>
      )}
    </header>
  );
};

export default Header;

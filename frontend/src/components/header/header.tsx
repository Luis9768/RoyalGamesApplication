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

  useEffect(() => {
    setAutenticado(verificarAutenticacao());
  }, []);

  function handleLogout() {
    logout();
    setAutenticado(false);
    notificacao("Você saiu da sua conta.");
    router.push("/home");
  }

  return (
    <header id={styles.header_container}>
      <Link href="/home" className={styles.logo_link}>
        <img
          src="/imgs/logo_royal.png"
          alt="Logo Royal Games"
          id={styles.imagem}
        />
      </Link>

      <div id={styles.direita}>
        <Link href="/catalogo">
          <button id={styles.botao_catalogo}>Catálogo</button>
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
            <button id={styles.botao_login}>Login</button>
          </Link>
        )}
      </div>
    </header>
  );
};

export default Header;

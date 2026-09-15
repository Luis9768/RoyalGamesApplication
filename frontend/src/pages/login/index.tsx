import { toast, ToastContainer } from "react-toastify";
import styles from "./login.module.css";
import { useState } from "react";
import { useRouter } from "next/router";
import { login } from "@/services/authService";



const Login = () => {

 const [email, setEmail] = useState<string>("");
    const [senha, setSenha] = useState<string>("");

    const router = useRouter();
    const notificacao = (msg: string) => toast(msg);
    const erro = (msg: string) => toast.error(msg);

    async function autenticar(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();
        try {
            await login(email, senha);
            notificacao("Login bem sucedido!")

            setTimeout(() => {
                router.push("/home");
            }, 2000); // 2 segundos

        } catch (error: any) {
            erro(error.message);
        }
    }

  return(
  <>
    <ToastContainer theme="dark" />
    <main id={styles.main}>
      <img src="/imgs/negona_robotica.png" alt="mulher robotica login" id={styles.imagem} />
      <div id={styles.campo_login} >
        <form id={styles.formulario}onSubmit={autenticar}>
          <img src="/imgs/logo_royal.png" alt="logo do royal" id={styles.imagem_login} />
          <div id={styles.campo_email}>
            <label htmlFor="email">Email</label>
            <input
              type="text"
              name="email"
              placeholder="email@exemplo.com"
              required
              value={email} 
              onChange={(e) => setEmail(e.target.value)}
              id={styles.caixa_email}
            />
          </div>
          <div id={styles.campo_senha}>
            <label htmlFor="password">Senha</label>
            <input
              type="password"
              name="senha"
              placeholder="*******"
              required
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
            />
          </div>
        <button id={styles.botao}>Entrar</button>
        </form>
      </div>
    </main>
  </>
  )
}
export default Login;

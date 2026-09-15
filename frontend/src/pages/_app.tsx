import "@/styles/globals.css";
import type { AppProps } from "next/app";
import Head from "next/head";
import { Orbitron, Exo_2 } from "next/font/google";
import { ToastContainer } from "react-toastify";

const orbitron = Orbitron({
    variable: "--fonte-titulo",
    weight: ["400", "500", "600", "700"],
    subsets: ["latin"],
});

const exo_2 = Exo_2({
    variable: "--fonte-padrao",
    weight: ["300", "400", "500", "600", "700"],
    subsets: ["latin"],
});

export default function App({ Component, pageProps }: AppProps) {
   return (
        <main className={`${orbitron.variable} ${exo_2.variable}`}>
            <Head>
                <title>Royal Games | A sua loja de jogos favorita</title>
                <meta name="description" content="Navegue por títulos de todas as gerações, descubra plataformas e gêneros na Royal Games." />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
            </Head>
            <Component {...pageProps} />
            <ToastContainer aria-label="Notificações" /> 
        </main>
   );
}

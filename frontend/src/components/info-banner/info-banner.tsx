import styles from "./info-banner.module.css";

const InfoBanner = () => {
    return (
        <section className={styles.secao_curiosidade}>
            <div className={styles.container_curiosidade}>
                
                <h2 className={styles.titulo_curiosidade}>
                    Jogos online podem afetar o comportamento humano?
                </h2>
                <div className={styles.linha_decorativa_larga}></div>

                <div className={styles.box_imagens}>
                    <img 
                        src="/imgs/lolzin.png" 
                        alt="Logo League of Legends" 
                        className={styles.img_jogo_logo} 
                    />
                    <img 
                        src="/imgs/cs.png" 
                        alt="Logo Counter-Strike Global Offensive" 
                        className={styles.img_jogo_logo} 
                    />
                </div>

                <p className={styles.texto_curiosidade}>
                    Estudos indicam que jogos podem alterar o comportamento humano...<br />
                    Principalmente quando o time resolve testar sua paciência em plena partida ranqueada.
                </p>

            </div>
        </section>
    );
};

export default InfoBanner;

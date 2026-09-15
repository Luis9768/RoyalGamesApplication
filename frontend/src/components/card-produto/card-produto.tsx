import styles from "@/components/card-produto/card-produto.module.css";
import { formatarPreco } from "@/utils/formatacao";
import Link from "next/link";

export type CardProdutoProps = {
  jogoId: number;
  titulo: string;
  descricao?: string;
  img: string;
  preco: number;
  autenticado?: boolean;
  onExcluir?: (id: number) => void;
};

const CardProduto = ({
  jogoId,
  titulo,
  img,
  preco,
  autenticado = false,
  onExcluir,
}: CardProdutoProps) => {
  return (
    <article className={styles.card_produto}>
      <Link href={`/detalhe-produto/${jogoId}`} className={styles.imagem_link}>
        <img
          src={img}
          alt={titulo}
          className={styles.imagem_card}
          onError={(e) => {
            (e.target as HTMLImageElement).src = "/imgs/banner.png";
          }}
        />
      </Link>

      <h3 className={styles.titulo} title={titulo}>{titulo}</h3>
      <p className={styles.preco}>{formatarPreco(preco)}</p>

      <div className={styles.botoesContainer}>
        <Link href={`/detalhe-produto/${jogoId}`}>
          <button className={styles.detalhes}>Detalhes</button>
        </Link>

        {autenticado && (
          <>
            <Link href={`/cadastro-jogo?id=${jogoId}`}>
              <button className={styles.btnEditar} title="Editar jogo">
                Editar
              </button>
            </Link>
            {onExcluir && (
              <button
                className={styles.btnExcluir}
                onClick={() => onExcluir(jogoId)}
                title="Inativar jogo"
              >
                Inativar
              </button>
            )}
          </>
        )}
      </div>
    </article>
  );
};

export default CardProduto;
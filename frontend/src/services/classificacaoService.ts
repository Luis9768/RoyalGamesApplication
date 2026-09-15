import { api } from "./api";

export interface Classificacao {
  classificacaoId: number;
  nomeClassificacao: string;
}

export async function listarClassificacoes(): Promise<Classificacao[]> {
  try {
    const response = await api.get("ClassificacaoIndicativa");
    return (response.data || []).map((item: any) => ({
      classificacaoId: item.classificacaoId ?? item.ClassificacaoIndicativaId ?? item.id,
      nomeClassificacao: item.nomeClassificacao ?? item.classificacao ?? item.Classificacao,
    }));
  } catch (error: any) {
    console.error("Erro ao listar classificações:", error);
    return [];
  }
}

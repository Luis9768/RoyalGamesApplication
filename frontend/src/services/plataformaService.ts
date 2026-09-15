import { api } from "./api";

export interface Plataforma {
  plataformaId: number;
  nome: string;
}

export async function listarPlataformas(): Promise<Plataforma[]> {
  try {
    const response = await api.get("Plataforma");
    return (response.data || []).map((item: any) => ({
      plataformaId: item.plataformaId ?? item.PlataformaId ?? item.id,
      nome: item.nome ?? item.Nome,
    }));
  } catch (error: any) {
    console.error("Erro ao listar plataformas:", error);
    return [];
  }
}

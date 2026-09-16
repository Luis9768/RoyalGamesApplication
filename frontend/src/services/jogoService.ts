import { api, resolveImageUrl } from "./api";

export interface Jogo {
  jogoId: number;
  nome: string;
  descricao: string;
  preco: number;
  statusJogo: boolean;
  imagemUrl: string;
  plataformaIds: number[];
  plataformas: string[];
  generoIds: number[];
  generos: string[];
  classificacaoId?: number;
  classificacao?: string;
  usuarioId?: number;
  usuarioNome?: string;
}

export interface JogoFormulario {
  nome: string;
  descricao: string;
  preco: string | number;
  statusJogo: boolean;
  imagem: File | null;
  plataformaIds: number[];
  generoIds: number[];
  classificacaoId: number;
}

export interface Genero {
  generoId: number;
  nome: string;
}

// Converte retorno bruto da API para o tipo padronizado Jogo
function normalizarJogo(raw: any): Jogo {
  const imgBruta = raw.imagemURL || raw.imagemUrl || raw.imagem;
  const precoRaw = raw.preco ?? raw.Preco ?? 0;
  const precoNumerico = typeof precoRaw === "number" ? precoRaw : parseFloat(precoRaw.toString().replace(",", ".")) || 0;
  return {
    jogoId: raw.jogoId ?? raw.JogoId ?? raw.id ?? 0,
    nome: raw.nome ?? raw.Nome ?? "",
    descricao: raw.descricao ?? raw.Descricao ?? "",
    preco: precoNumerico,
    statusJogo: raw.statusJogo ?? raw.StatusJogo ?? true,
    imagemUrl: resolveImageUrl(imgBruta),
    plataformaIds: raw.plataformaIds ?? raw.PlataformaIds ?? [],
    plataformas: raw.plataforma ?? raw.Plataforma ?? raw.plataformas ?? [],
    generoIds: raw.generoIds ?? raw.GeneroIds ?? [],
    generos: raw.genero ?? raw.Genero ?? raw.generos ?? [],
    classificacaoId: raw.classificacaoId ?? raw.ClassificacaoId ?? raw.classificaçãoIdFK,
    classificacao: raw.classificacao ?? raw.Classificacao ?? "",
    usuarioId: raw.usuarioId ?? raw.UsuarioId,
    usuarioNome: raw.usuarioNome ?? raw.UsuarioNome,
  };
}

export async function listarJogo(): Promise<Jogo[]> {
  try {
    const response = await api.get("Jogo");
    const lista = Array.isArray(response.data) ? response.data : [];
    return lista.map(normalizarJogo);
  } catch (error: any) {
    console.error("Erro ao listar jogos:", error);
    throw new Error(error.response?.data?.message || error.response?.data || "Erro ao buscar jogos");
  }
}

export async function listarPorId(id: number): Promise<Jogo> {
  try {
    const response = await api.get(`Jogo/${id}`);
    return normalizarJogo(response.data);
  } catch (error: any) {
    console.error(`Erro ao buscar jogo ${id}:`, error);
    throw new Error(error.response?.data?.message || error.response?.data || "Jogo não encontrado");
  }
}

export async function cadastrarJogo(dados: JogoFormulario): Promise<void> {
  try {
    const formData = new FormData();
    formData.append("nome", dados.nome);
    formData.append("descricao", dados.descricao);
    formData.append("preco", dados.preco.toString().replace(",", "."));
    formData.append("statusJogo", dados.statusJogo ? "true" : "false");

    if (dados.imagem) {
      formData.append("imagem", dados.imagem);
    }

    dados.plataformaIds.forEach((platId) => {
      formData.append("plataformaIds", platId.toString());
    });

    dados.generoIds.forEach((genId) => {
      formData.append("generoIds", genId.toString());
    });

    if (dados.classificacaoId) {
      formData.append("classificacaoId", dados.classificacaoId.toString());
    }

    await api.post("Jogo", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  } catch (error: any) {
    console.error("Erro ao cadastrar jogo:", error);
    const msg = error.response?.data?.message || error.response?.data || error.message || "Erro ao cadastrar jogo";
    throw new Error(typeof msg === "string" ? msg : JSON.stringify(msg));
  }
}

export async function editarJogo(jogoId: number, dados: JogoFormulario): Promise<void> {
  try {
    const formData = new FormData();
    formData.append("nome", dados.nome);
    formData.append("descricao", dados.descricao);
    formData.append("preco", dados.preco.toString().replace(",", "."));
    formData.append("statusJogo", dados.statusJogo ? "true" : "false");

    if (dados.imagem) {
      formData.append("imagem", dados.imagem);
    }

    dados.plataformaIds.forEach((platId) => {
      formData.append("plataformaIds", platId.toString());
    });

    dados.generoIds.forEach((genId) => {
      formData.append("generoIds", genId.toString());
    });

    if (dados.classificacaoId) {
      formData.append("classificacaoId", dados.classificacaoId.toString());
    }

    await api.put(`Jogo/${jogoId}`, formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });
  } catch (error: any) {
    console.error(`Erro ao editar jogo ${jogoId}:`, error);
    const msg = error.response?.data?.message || error.response?.data || error.message || "Erro ao atualizar jogo";
    throw new Error(typeof msg === "string" ? msg : JSON.stringify(msg));
  }
}

export async function excluirJogo(jogoId: number): Promise<void> {
  try {
    await api.delete(`Jogo/${jogoId}`);
  } catch (error: any) {
    console.error(`Erro ao inativar jogo ${jogoId}:`, error);
    const msg = error.response?.data?.message || error.response?.data || "Erro ao excluir jogo";
    throw new Error(typeof msg === "string" ? msg : JSON.stringify(msg));
  }
}

export async function listarGeneros(): Promise<Genero[]> {
  try {
    const response = await api.get("Genero");
    return (response.data || []).map((item: any) => ({
      generoId: item.generoId ?? item.GeneroId ?? item.id,
      nome: item.nome ?? item.Nome,
    }));
  } catch (error: any) {
    console.error("Erro ao listar gêneros:", error);
    return [];
  }
}

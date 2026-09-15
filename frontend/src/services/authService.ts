import secureLocalStorage from "react-secure-storage";
import { api } from "./api";

export async function login(email: string, senha: string) {
  try {
    const response = await api.post("Autenticacao/login", { email, senha });
    const token = response.data.token || response.data.Token || response.data;
    if (typeof window !== "undefined") {
      secureLocalStorage.setItem("Token", token);
    }
    return token;
  } catch (error: any) {
    const mensagem =
      error.response?.data?.message ||
      error.response?.data ||
      error.message ||
      "Email ou senha inválidos";
    throw new Error(typeof mensagem === "string" ? mensagem : "Erro ao autenticar");
  }
}

export function logout() {
  if (typeof window !== "undefined") {
    secureLocalStorage.removeItem("Token");
  }
}

export function obterToken(): string | null {
  if (typeof window !== "undefined") {
    return (secureLocalStorage.getItem("Token") as string) || null;
  }
  return null;
}

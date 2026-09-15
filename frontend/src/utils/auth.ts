import secureLocalStorage from "react-secure-storage";

export function verificarAutenticacao(){
    const token = secureLocalStorage.getItem("Token");
    return !!token;

    //token passa a ser booleano
    // se existir informacao dentro do token, ele retona TRUE
    //se NAO existir info, retorna FALSE
}
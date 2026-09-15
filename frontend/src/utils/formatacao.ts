export function formatarPreco(valor: number){
    return valor.toLocaleString("pt-BR", {
       style: "currency",
       currency: "BRL"
    })
}

// 25.00
// R$25,00
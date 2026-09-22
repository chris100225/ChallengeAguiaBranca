package br.com.fiap.aguia.domain.models

// Representa a estrutura de dados de uma Ideia/Problema
data class Ideia(
    val id: String = "",
    val titulo: String,
    val descricao: String,
    val status: String // Ex: "Pendente", "Em Análise", "Aprovada", "Rejeitada"
)
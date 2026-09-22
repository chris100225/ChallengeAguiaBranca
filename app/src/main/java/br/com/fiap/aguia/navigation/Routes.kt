package br.com.fiap.aguia.navigation

sealed class AppRoute(val route: String) {
    object Login : AppRoute("login")
    object Cadastro : AppRoute("cadastro")
    object Operacional : AppRoute("operacional_home")
    object NovaIdeia : AppRoute("nova_ideia")
    object Gestor : AppRoute("gestor_home")
    object Lideranca : AppRoute("lideranca_home")

    object Orientacoes : AppRoute("orientacoes/{perfil}") {
        fun createRoute(perfil: String) = "orientacoes/$perfil"
    }

    object Projetos : AppRoute("projetos/{perfil}") {
        fun createRoute(perfil: String) = "projetos/$perfil"
    }
}
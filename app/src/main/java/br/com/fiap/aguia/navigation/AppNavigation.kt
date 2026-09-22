package br.com.fiap.aguia.navigation

import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import br.com.fiap.aguia.features.auth.CadastroScreen
import br.com.fiap.aguia.features.auth.LoginScreen
import br.com.fiap.aguia.features.gestor.GestorScreen
import br.com.fiap.aguia.features.gestor.ProjetosScreen
import br.com.fiap.aguia.features.lideranca.LiderancaScreen
import br.com.fiap.aguia.features.operacional.NovaIdeiaScreen
import br.com.fiap.aguia.features.operacional.OperacionalScreen
import br.com.fiap.aguia.features.orientacoes.OrientacoesScreen
import br.com.fiap.aguia.data.FirebaseRepository
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import kotlinx.coroutines.launch

@Composable
fun AppNavigation() {
    val navController = rememberNavController()

    NavHost(
        navController = navController,
        startDestination = AppRoute.Login.route
    ) {

        composable(AppRoute.Login.route) {
            LoginScreen(
                onLoginSuccess = { role ->
                    val destination = when (role.lowercase()) {
                        "operacional" -> AppRoute.Operacional.route
                        "gestor" -> AppRoute.Gestor.route
                        "lideranca" -> AppRoute.Lideranca.route
                        else -> AppRoute.Login.route
                    }
                    navController.navigate(destination) {
                        popUpTo(AppRoute.Login.route) { inclusive = true }
                    }
                },
                onNavigateToCadastro = {
                    navController.navigate(AppRoute.Cadastro.route)
                }
            )
        }

        composable(AppRoute.Cadastro.route) {
            CadastroScreen(
                onNavigateBack = { navController.popBackStack() },
                onCadastroSuccess = { navController.popBackStack() }
            )
        }

        composable(AppRoute.Operacional.route) {
            OperacionalScreen(
                onNavigateToNovaIdeia = {
                    navController.navigate(AppRoute.NovaIdeia.route)
                },
                onNavigateToOrientacoes = {
                    navController.navigate(AppRoute.Orientacoes.createRoute("operacional"))
                }
            )
        }

        composable(AppRoute.NovaIdeia.route) {
            val repository = remember { FirebaseRepository() }
            val scope = rememberCoroutineScope()
            NovaIdeiaScreen(
                onNavigateBack = { navController.popBackStack() },
                onSaveIdeia = { titulo, descricao, area, temGanho, valorEstimado ->
                    scope.launch {
                        repository.salvarIdeia(titulo, descricao, area, temGanho, valorEstimado)
                        navController.popBackStack()
                    }
                }
            )
        }

        composable(AppRoute.Gestor.route) {
            GestorScreen(
                onNavigateToOrientacoes = {
                    navController.navigate(AppRoute.Orientacoes.createRoute("gestor"))
                },
                onNavigateToProjetos = {
                    navController.navigate(AppRoute.Projetos.createRoute("gestor"))
                }
            )
        }

        composable(AppRoute.Lideranca.route) {
            LiderancaScreen(
                onNavigateToOrientacoes = {
                    navController.navigate(AppRoute.Orientacoes.createRoute("lideranca"))
                },
                onNavigateToProjetos = {
                    navController.navigate(AppRoute.Projetos.createRoute("lideranca"))
                }
            )
        }

        composable("orientacoes/{perfil}") { backStackEntry ->
            val perfil = backStackEntry.arguments?.getString("perfil") ?: "operacional"
            OrientacoesScreen(
                isLideranca = perfil == "lideranca",
                onNavigateBack = { navController.popBackStack() }
            )
        }

        // Nova rota de Projetos
        composable("projetos/{perfil}") { backStackEntry ->
            val perfil = backStackEntry.arguments?.getString("perfil") ?: "gestor"
            ProjetosScreen(
                isLideranca = perfil == "lideranca",
                onNavigateBack = { navController.popBackStack() }
            )
        }
    }
}
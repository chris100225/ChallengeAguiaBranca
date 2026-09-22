package br.com.fiap.aguia.features.operacional

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Info
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import br.com.fiap.aguia.domain.models.Ideia
import br.com.fiap.aguia.ui.theme.*

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun OperacionalScreen(
    onNavigateToNovaIdeia: () -> Unit = {},
    onNavigateToOrientacoes: () -> Unit = {}
) {
    val ideiasMock = listOf(
        Ideia("1", "Melhoria na Iluminação da Frota", "Instalar LEDs nos autocarros mais antigos.", "Aprovada"),
        Ideia("2", "App para Escalas", "Digitalizar o papel das escalas dos motoristas.", "Em Análise"),
        Ideia("3", "Revisão de Pneus", "Alterar o fornecedor de pneus devido ao desgaste rápido.", "Pendente")
    )

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Minhas Ideias", color = SurfaceWhite) },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = PrimaryBlue
                ),
                actions = {
                    IconButton(onClick = onNavigateToOrientacoes) {
                        Icon(imageVector = Icons.Default.Info, contentDescription = "Orientações", tint = SurfaceWhite)
                    }
                }
            )
        },
        floatingActionButton = {
            FloatingActionButton(
                onClick = onNavigateToNovaIdeia,
                containerColor = SecondaryBlue,
                contentColor = SurfaceWhite
            ) {
                Icon(imageVector = Icons.Default.Add, contentDescription = "Adicionar Nova Ideia")
            }
        },
        containerColor = BackgroundLight
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
        ) {
            // Faixa amarela da identidade Águia Branca
            HorizontalDivider(
                thickness = 4.dp,
                color = PrimaryYellow
            )

            LazyColumn(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(horizontal = 16.dp),
                verticalArrangement = Arrangement.spacedBy(12.dp),
                contentPadding = PaddingValues(top = 16.dp, bottom = 80.dp)
            ) {
                items(ideiasMock) { ideia ->
                    IdeiaCard(ideia = ideia)
                }
            }
        }
    }
}

@Composable
fun IdeiaCard(ideia: Ideia) {
    val statusColor = when (ideia.status) {
        "Aprovada" -> SuccessGreen
        "Em Análise" -> WarningYellow
        "Rejeitada" -> ErrorRed
        else -> TextSecondary
    }

    Card(
        modifier = Modifier.fillMaxWidth(),
        shape = AppShapes.medium,
        colors = CardDefaults.cardColors(containerColor = SurfaceWhite),
        elevation = CardDefaults.cardElevation(defaultElevation = 2.dp)
    ) {
        Column(modifier = Modifier.padding(16.dp)) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.CenterVertically
            ) {
                Text(
                    text = ideia.titulo,
                    style = AppTypography.titleLarge.copy(fontSize = 18.sp),
                    modifier = Modifier.weight(1f)
                )
                Box(
                    modifier = Modifier
                        .background(color = statusColor.copy(alpha = 0.2f), shape = AppShapes.small)
                        .padding(horizontal = 8.dp, vertical = 4.dp)
                ) {
                    Text(
                        text = ideia.status,
                        style = AppTypography.labelMedium,
                        color = statusColor,
                        fontWeight = FontWeight.Bold
                    )
                }
            }

            Spacer(modifier = Modifier.height(8.dp))

            Text(
                text = ideia.descricao,
                style = AppTypography.bodyLarge,
                color = TextSecondary,
                maxLines = 2
            )
        }
    }
}

@Preview(showBackground = true)
@Composable
fun PreviewOperacionalScreen() {
    AguiaBrancaTheme {
        OperacionalScreen()
    }
}
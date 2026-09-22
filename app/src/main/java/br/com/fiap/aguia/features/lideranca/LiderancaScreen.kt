package br.com.fiap.aguia.features.lideranca

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.TrendingUp
import androidx.compose.material.icons.filled.AttachMoney
import androidx.compose.material.icons.filled.CheckCircle
import androidx.compose.material.icons.filled.Info
import androidx.compose.material.icons.filled.Lightbulb
import androidx.compose.material.icons.filled.List
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import br.com.fiap.aguia.ui.theme.*

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun LiderancaScreen(
    onNavigateToOrientacoes: () -> Unit = {},
    onNavigateToProjetos: () -> Unit = {}
) {
    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Visão Estratégica", color = SurfaceWhite) },
                colors = TopAppBarDefaults.topAppBarColors(
                    containerColor = PrimaryBlue
                ),
                actions = {
                    IconButton(onClick = onNavigateToProjetos) {
                        Icon(imageVector = Icons.Default.List, contentDescription = "Projetos", tint = SurfaceWhite)
                    }
                    IconButton(onClick = onNavigateToOrientacoes) {
                        Icon(imageVector = Icons.Default.Info, contentDescription = "Orientações", tint = SurfaceWhite)
                    }
                }
            )
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

            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .verticalScroll(rememberScrollState())
                    .padding(16.dp),
                verticalArrangement = Arrangement.spacedBy(24.dp)
            ) {
                Text(
                    text = "Resumo de Indicadores",
                    style = AppTypography.titleLarge,
                    color = TextPrimary
                )

                Column(verticalArrangement = Arrangement.spacedBy(16.dp)) {
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.spacedBy(16.dp)
                    ) {
                        KpiCard(
                            modifier = Modifier.weight(1f),
                            titulo = "Ideias Geradas",
                            valor = "142",
                            icone = Icons.Default.Lightbulb,
                            corDestaque = WarningYellow
                        )
                        KpiCard(
                            modifier = Modifier.weight(1f),
                            titulo = "Projetos Ativos",
                            valor = "28",
                            icone = Icons.Default.CheckCircle,
                            corDestaque = SuccessGreen
                        )
                    }
                    Row(
                        modifier = Modifier.fillMaxWidth(),
                        horizontalArrangement = Arrangement.spacedBy(16.dp)
                    ) {
                        KpiCard(
                            modifier = Modifier.weight(1f),
                            titulo = "ROI Estimado",
                            valor = "R$ 1.2M",
                            icone = Icons.Default.AttachMoney,
                            corDestaque = PrimaryBlue
                        )
                        KpiCard(
                            modifier = Modifier.weight(1f),
                            titulo = "Taxa Aprovação",
                            valor = "34%",
                            icone = Icons.AutoMirrored.Filled.TrendingUp,
                            corDestaque = SecondaryBlue
                        )
                    }
                }

                Text(
                    text = "Alinhamento com Diretrizes",
                    style = AppTypography.titleLarge,
                    color = TextPrimary,
                    modifier = Modifier.padding(top = 8.dp)
                )

                Card(
                    modifier = Modifier.fillMaxWidth(),
                    shape = AppShapes.medium,
                    colors = CardDefaults.cardColors(containerColor = SurfaceWhite),
                    elevation = CardDefaults.cardElevation(defaultElevation = 2.dp)
                ) {
                    Column(
                        modifier = Modifier.padding(16.dp),
                        verticalArrangement = Arrangement.spacedBy(16.dp)
                    ) {
                        MetaProgressBar(
                            titulo = "Redução de Custos Operacionais",
                            progresso = 0.75f,
                            cor = SuccessGreen
                        )
                        MetaProgressBar(
                            titulo = "Inovação e Transformação Digital",
                            progresso = 0.60f,
                            cor = SecondaryBlue
                        )
                        MetaProgressBar(
                            titulo = "Sustentabilidade (ESG)",
                            progresso = 0.35f,
                            cor = WarningYellow
                        )
                    }
                }
            }
        }
    }
}

@Composable
fun KpiCard(
    modifier: Modifier = Modifier,
    titulo: String,
    valor: String,
    icone: ImageVector,
    corDestaque: Color
) {
    Card(
        modifier = modifier.height(110.dp),
        shape = AppShapes.medium,
        colors = CardDefaults.cardColors(containerColor = SurfaceWhite),
        elevation = CardDefaults.cardElevation(defaultElevation = 2.dp)
    ) {
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(12.dp),
            verticalArrangement = Arrangement.SpaceBetween
        ) {
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween,
                verticalAlignment = Alignment.Top
            ) {
                Text(
                    text = titulo,
                    style = AppTypography.labelMedium,
                    color = TextSecondary,
                    modifier = Modifier.weight(1f)
                )
                Box(
                    modifier = Modifier
                        .size(32.dp)
                        .clip(CircleShape)
                        .background(corDestaque.copy(alpha = 0.1f)),
                    contentAlignment = Alignment.Center
                ) {
                    Icon(
                        imageVector = icone,
                        contentDescription = null,
                        tint = corDestaque,
                        modifier = Modifier.size(18.dp)
                    )
                }
            }
            Text(
                text = valor,
                style = AppTypography.headlineMedium.copy(fontSize = 24.sp),
                color = TextPrimary
            )
        }
    }
}

@Composable
fun MetaProgressBar(
    titulo: String,
    progresso: Float,
    cor: Color
) {
    Column(modifier = Modifier.fillMaxWidth()) {
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Text(
                text = titulo,
                style = AppTypography.bodyLarge,
                color = TextPrimary
            )
            Text(
                text = "${(progresso * 100).toInt()}%",
                style = AppTypography.bodyLarge.copy(fontWeight = FontWeight.Bold),
                color = cor
            )
        }
        Spacer(modifier = Modifier.height(8.dp))
        LinearProgressIndicator(
            progress = { progresso },
            modifier = Modifier
                .fillMaxWidth()
                .height(8.dp)
                .clip(AppShapes.small),
            color = cor,
            trackColor = BackgroundLight,
        )
    }
}

@Preview(showBackground = true)
@Composable
fun PreviewLiderancaScreen() {
    AguiaBrancaTheme {
        LiderancaScreen(
            onNavigateToOrientacoes = {}
        )
    }
}
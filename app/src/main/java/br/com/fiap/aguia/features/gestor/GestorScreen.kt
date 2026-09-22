package br.com.fiap.aguia.features.gestor

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Check
import androidx.compose.material.icons.filled.Close
import androidx.compose.material.icons.filled.Info
import androidx.compose.material.icons.filled.List
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import br.com.fiap.aguia.ui.theme.*

data class IdeiaGestorMock(
    val id: String,
    val titulo: String,
    val descricao: String,
    val area: String,
    val autor: String,
    val status: String
)

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun GestorScreen(
    onNavigateToOrientacoes: () -> Unit = {},
    onNavigateToProjetos: () -> Unit = {}
) {
    var tabSelecionada by remember { mutableIntStateOf(0) }
    val tabs = listOf("Pendentes", "Em Análise", "Aprovadas")

    val ideiasMock = remember {
        listOf(
            IdeiaGestorMock("1", "Melhoria na Iluminação da Frota", "Instalar LEDs nos autocarros.", "Manutenção", "João Silva", "Pendentes"),
            IdeiaGestorMock("2", "Novo fardamento", "Tecidos mais respiráveis para o verão.", "RH", "Maria Santos", "Pendentes"),
            IdeiaGestorMock("3", "App para Escalas", "Digitalizar o papel das escalas.", "TI", "Carlos Souza", "Em Análise"),
            IdeiaGestorMock("4", "Revisão de Pneus", "Alterar fornecedor devido ao desgaste.", "Logística", "Ana Costa", "Aprovadas")
        )
    }

    val ideiasFiltradas = ideiasMock.filter { it.status == tabs[tabSelecionada] }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Gestão de Ideias", color = SurfaceWhite) },
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

            TabRow(
                selectedTabIndex = tabSelecionada,
                containerColor = SurfaceWhite,
                contentColor = PrimaryBlue
            ) {
                tabs.forEachIndexed { index, title ->
                    Tab(
                        selected = tabSelecionada == index,
                        onClick = { tabSelecionada = index },
                        text = {
                            Text(
                                text = title,
                                fontWeight = if (tabSelecionada == index) FontWeight.Bold else FontWeight.Normal
                            )
                        }
                    )
                }
            }

            LazyColumn(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(horizontal = 16.dp),
                contentPadding = PaddingValues(top = 16.dp, bottom = 16.dp),
                verticalArrangement = Arrangement.spacedBy(16.dp)
            ) {
                if (ideiasFiltradas.isEmpty()) {
                    item {
                        Text(
                            text = "Nenhuma ideia nesta coluna.",
                            modifier = Modifier.padding(16.dp),
                            color = TextSecondary,
                            style = AppTypography.bodyLarge
                        )
                    }
                } else {
                    items(ideiasFiltradas) { ideia ->
                        IdeiaCardGestor(
                            ideia = ideia,
                            onAprovar = {},
                            onRejeitar = {},
                            onAnalisar = {}
                        )
                    }
                }
            }
        }
    }
}

@Composable
fun IdeiaCardGestor(
    ideia: IdeiaGestorMock,
    onAprovar: () -> Unit,
    onRejeitar: () -> Unit,
    onAnalisar: () -> Unit
) {
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
                Box(
                    modifier = Modifier
                        .background(color = SecondaryBlue.copy(alpha = 0.1f), shape = AppShapes.small)
                        .padding(horizontal = 8.dp, vertical = 4.dp)
                ) {
                    Text(
                        text = ideia.area,
                        style = AppTypography.labelMedium,
                        color = SecondaryBlue,
                        fontWeight = FontWeight.Bold
                    )
                }
                Text(
                    text = "Por: ${ideia.autor}",
                    style = AppTypography.labelMedium,
                    color = TextSecondary
                )
            }

            Spacer(modifier = Modifier.height(12.dp))

            Text(
                text = ideia.titulo,
                style = AppTypography.titleLarge.copy(fontSize = 18.sp),
                color = TextPrimary
            )
            Spacer(modifier = Modifier.height(8.dp))
            Text(
                text = ideia.descricao,
                style = AppTypography.bodyLarge,
                color = TextSecondary
            )

            Spacer(modifier = Modifier.height(16.dp))
            HorizontalDivider(color = BackgroundLight)
            Spacer(modifier = Modifier.height(8.dp))

            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.End
            ) {
                when (ideia.status) {
                    "Pendentes" -> {
                        OutlinedButton(
                            onClick = onAnalisar,
                            modifier = Modifier.padding(end = 8.dp)
                        ) {
                            Icon(Icons.Default.Search, contentDescription = null, modifier = Modifier.size(16.dp))
                            Spacer(modifier = Modifier.width(4.dp))
                            Text("Analisar")
                        }
                        Button(
                            onClick = onAprovar,
                            colors = ButtonDefaults.buttonColors(containerColor = SuccessGreen)
                        ) {
                            Icon(Icons.Default.Check, contentDescription = null, modifier = Modifier.size(16.dp))
                            Spacer(modifier = Modifier.width(4.dp))
                            Text("Aprovar")
                        }
                    }
                    "Em Análise" -> {
                        OutlinedButton(
                            onClick = onRejeitar,
                            modifier = Modifier.padding(end = 8.dp),
                            colors = ButtonDefaults.outlinedButtonColors(contentColor = ErrorRed)
                        ) {
                            Icon(Icons.Default.Close, contentDescription = null, modifier = Modifier.size(16.dp))
                            Spacer(modifier = Modifier.width(4.dp))
                            Text("Rejeitar")
                        }
                        Button(
                            onClick = onAprovar,
                            colors = ButtonDefaults.buttonColors(containerColor = SuccessGreen)
                        ) {
                            Icon(Icons.Default.Check, contentDescription = null, modifier = Modifier.size(16.dp))
                            Spacer(modifier = Modifier.width(4.dp))
                            Text("Aprovar")
                        }
                    }
                    "Aprovadas" -> {
                        Text(
                            text = "Projeto em planeamento",
                            style = AppTypography.labelMedium,
                            color = SuccessGreen,
                            fontWeight = FontWeight.Bold,
                            modifier = Modifier.padding(vertical = 8.dp)
                        )
                    }
                }
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun PreviewGestorScreen() {
    AguiaBrancaTheme {
        GestorScreen()
    }
}
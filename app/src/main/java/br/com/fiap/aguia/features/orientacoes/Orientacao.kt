package br.com.fiap.aguia.features.orientacoes

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material.icons.filled.Add
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import br.com.fiap.aguia.ui.theme.*

data class Orientacao(val id: String, val titulo: String, val descricao: String)

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun OrientacoesScreen(
    isLideranca: Boolean,
    onNavigateBack: () -> Unit
) {
    var orientacoes by remember {
        mutableStateOf(
            listOf(
                Orientacao("1", "Redução de Custos Operacionais", "Foco em ideias que otimizem o consumo de combustível e manutenção da frota."),
                Orientacao("2", "Transformação Digital", "Priorizar a digitalização de processos manuais e uso de papel nas garagens."),
                Orientacao("3", "Sustentabilidade ESG", "Iniciativas que reduzam o impacto ambiental das nossas operações diárias.")
            )
        )
    }

    var showDialog by remember { mutableStateOf(false) }
    var novoTitulo by remember { mutableStateOf("") }
    var novaDescricao by remember { mutableStateOf("") }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Orientações Estratégicas", color = SurfaceWhite) },
                navigationIcon = {
                    IconButton(onClick = onNavigateBack) {
                        Icon(Icons.AutoMirrored.Filled.ArrowBack, contentDescription = "Voltar", tint = SurfaceWhite)
                    }
                },
                colors = TopAppBarDefaults.topAppBarColors(containerColor = PrimaryBlue)
            )
        },
        floatingActionButton = {
            if (isLideranca) {
                FloatingActionButton(
                    onClick = { showDialog = true },
                    containerColor = SecondaryBlue,
                    contentColor = SurfaceWhite
                ) {
                    Icon(Icons.Default.Add, contentDescription = "Adicionar Orientação")
                }
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
                    .padding(16.dp),
                verticalArrangement = Arrangement.spacedBy(16.dp)
            ) {
                item {
                    Text(
                        text = "Diretrizes atuais para guiar novas ideias e projetos.",
                        style = AppTypography.bodyLarge,
                        color = TextSecondary,
                        modifier = Modifier.padding(bottom = 8.dp)
                    )
                }

                if (orientacoes.isEmpty()) {
                    item { Text("Nenhuma orientação cadastrada.", color = TextSecondary) }
                } else {
                    items(orientacoes) { orientacao ->
                        OrientacaoCard(orientacao)
                    }
                }
            }
        }

        if (showDialog) {
            AlertDialog(
                onDismissRequest = { showDialog = false },
                title = { Text("Nova Orientação", color = PrimaryBlue) },
                text = {
                    Column(verticalArrangement = Arrangement.spacedBy(12.dp)) {
                        OutlinedTextField(
                            value = novoTitulo,
                            onValueChange = { novoTitulo = it },
                            label = { Text("Título") },
                            singleLine = true,
                            shape = AppShapes.small
                        )
                        OutlinedTextField(
                            value = novaDescricao,
                            onValueChange = { novaDescricao = it },
                            label = { Text("Descrição e Objetivos") },
                            modifier = Modifier.height(120.dp),
                            maxLines = 4,
                            shape = AppShapes.small
                        )
                    }
                },
                confirmButton = {
                    Button(
                        onClick = {
                            if (novoTitulo.isNotBlank() && novaDescricao.isNotBlank()) {
                                orientacoes = orientacoes + Orientacao(
                                    id = System.currentTimeMillis().toString(),
                                    titulo = novoTitulo,
                                    descricao = novaDescricao
                                )
                                novoTitulo = ""
                                novaDescricao = ""
                                showDialog = false
                            }
                        },
                        colors = ButtonDefaults.buttonColors(containerColor = PrimaryBlue)
                    ) {
                        Text("Salvar")
                    }
                },
                dismissButton = {
                    TextButton(onClick = { showDialog = false }) {
                        Text("Cancelar", color = TextSecondary)
                    }
                }
            )
        }
    }
}

@Composable
fun OrientacaoCard(orientacao: Orientacao) {
    Card(
        modifier = Modifier.fillMaxWidth(),
        colors = CardDefaults.cardColors(containerColor = SurfaceWhite),
        elevation = CardDefaults.cardElevation(defaultElevation = 2.dp),
        shape = AppShapes.medium
    ) {
        Column(modifier = Modifier.padding(16.dp)) {
            Text(text = orientacao.titulo, style = AppTypography.titleLarge, color = PrimaryBlue)
            Spacer(modifier = Modifier.height(8.dp))
            Text(text = orientacao.descricao, style = AppTypography.bodyLarge, color = TextSecondary)
        }
    }
}

@Preview(showBackground = true)
@Composable
fun PreviewOrientacoesLideranca() {
    AguiaBrancaTheme {
        OrientacoesScreen(isLideranca = true, onNavigateBack = {})
    }
}

@Preview(showBackground = true)
@Composable
fun PreviewOrientacoesOperacional() {
    AguiaBrancaTheme {
        OrientacoesScreen(isLideranca = false, onNavigateBack = {})
    }
}
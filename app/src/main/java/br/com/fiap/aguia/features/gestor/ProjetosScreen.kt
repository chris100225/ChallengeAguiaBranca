package br.com.fiap.aguia.features.gestor

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.ArrowDropDown
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import br.com.fiap.aguia.ui.theme.*
import androidx.compose.material.icons.automirrored.filled.ArrowBack

data class Projeto(
    val id: String,
    val titulo: String,
    val descricao: String,
    val responsavel: String,
    val etapa: String,
    val status: String,
    val investimento: String,
    val retorno: String,
    val prazo: String
)

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ProjetosScreen(
    isLideranca: Boolean = false,
    onNavigateBack: () -> Unit = {}
) {
    var showDialog by remember { mutableStateOf(false) }

    val projetosMock = remember {
        mutableStateListOf(
            Projeto("1", "Modernização da Frota", "Substituição de 20 veículos antigos por modelos mais eficientes.",
                "Carlos Souza", "Em Execução", "No Prazo", "R$ 800.000", "R$ 1.200.000", "Dez/2025"),
            Projeto("2", "App de Escalas Digitais", "Digitalizar o controle de escalas dos motoristas.",
                "Ana Costa", "Planejamento", "No Prazo", "R$ 45.000", "R$ 120.000", "Mar/2026"),
            Projeto("3", "Redução de Consumo de Combustível", "Treinamento de motoristas para direção econômica.",
                "João Silva", "Concluído", "No Prazo", "R$ 12.000", "R$ 85.000", "Jan/2025")
        )
    }

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Projetos", color = SurfaceWhite) },
                navigationIcon = {
                    IconButton(onClick = onNavigateBack) {
                        Icon(
                            imageVector = Icons.AutoMirrored.Filled.ArrowBack,
                            contentDescription = "Voltar",
                            tint = SurfaceWhite
                        )
                    }
                },
                colors = TopAppBarDefaults.topAppBarColors(containerColor = PrimaryBlue)
            )
        },
        floatingActionButton = {
            if (!isLideranca) {
                FloatingActionButton(
                    onClick = { showDialog = true },
                    containerColor = SecondaryBlue,
                    contentColor = SurfaceWhite
                ) {
                    Icon(Icons.Default.Add, contentDescription = "Novo Projeto")
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
                    .padding(horizontal = 16.dp),
                verticalArrangement = Arrangement.spacedBy(16.dp),
                contentPadding = PaddingValues(top = 16.dp, bottom = 80.dp)
            ) {
                items(projetosMock) { projeto ->
                    ProjetoCard(projeto = projeto, isLideranca = isLideranca)
                }
            }
        }
    }

    if (showDialog) {
        NovoprojetoDialog(
            onDismiss = { showDialog = false },
            onSave = { novo ->
                projetosMock.add(novo)
                showDialog = false
            }
        )
    }
}

@Composable
fun ProjetoCard(projeto: Projeto, isLideranca: Boolean) {
    val etapaColor = when (projeto.etapa) {
        "Concluído" -> SuccessGreen
        "Em Execução" -> SecondaryBlue
        else -> WarningYellow
    }
    val statusColor = when (projeto.status) {
        "No Prazo" -> SuccessGreen
        "Atrasado" -> ErrorRed
        else -> WarningYellow
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
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                Badge(etapa = projeto.etapa, cor = etapaColor)
                Badge(etapa = projeto.status, cor = statusColor)
            }

            Spacer(modifier = Modifier.height(12.dp))

            Text(
                text = projeto.titulo,
                style = AppTypography.titleLarge.copy(fontSize = 18.sp),
                color = TextPrimary
            )
            Text(
                text = projeto.descricao,
                style = AppTypography.bodyLarge,
                color = TextSecondary,
                modifier = Modifier.padding(top = 4.dp)
            )

            Spacer(modifier = Modifier.height(12.dp))
            HorizontalDivider(color = BackgroundLight)
            Spacer(modifier = Modifier.height(12.dp))

            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                InfoItem(label = "Investimento", valor = projeto.investimento)
                InfoItem(label = "Retorno", valor = projeto.retorno)
                InfoItem(label = "Prazo", valor = projeto.prazo)
            }

            Spacer(modifier = Modifier.height(8.dp))

            Text(
                text = "Responsável: ${projeto.responsavel}",
                style = AppTypography.labelMedium,
                color = TextSecondary
            )
        }
    }
}

@Composable
fun Badge(etapa: String, cor: Color) {
    Box(
        modifier = Modifier
            .background(cor.copy(alpha = 0.15f), shape = AppShapes.small)
            .padding(horizontal = 10.dp, vertical = 4.dp)
    ) {
        Text(
            text = etapa,
            style = AppTypography.labelMedium,
            color = cor,
            fontWeight = FontWeight.Bold
        )
    }
}

@Composable
fun InfoItem(label: String, valor: String) {
    Column(horizontalAlignment = Alignment.CenterHorizontally) {
        Text(text = label, style = AppTypography.labelMedium, color = TextSecondary)
        Text(
            text = valor,
            style = AppTypography.bodyLarge.copy(fontWeight = FontWeight.Bold),
            color = TextPrimary
        )
    }
}

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun NovoprojetoDialog(onDismiss: () -> Unit, onSave: (Projeto) -> Unit) {
    var titulo by remember { mutableStateOf("") }
    var descricao by remember { mutableStateOf("") }
    var responsavel by remember { mutableStateOf("") }
    var investimento by remember { mutableStateOf("") }
    var retorno by remember { mutableStateOf("") }
    var prazo by remember { mutableStateOf("") }
    var etapaSelecionada by remember { mutableStateOf("Planejamento") }
    var dropdownExpanded by remember { mutableStateOf(false) }
    val etapas = listOf("Planejamento", "Em Execução", "Concluído")

    AlertDialog(
        onDismissRequest = onDismiss,
        title = { Text("Novo Projeto", color = PrimaryBlue) },
        text = {
            Column(
                verticalArrangement = Arrangement.spacedBy(10.dp),
                modifier = Modifier.fillMaxWidth()
            ) {
                OutlinedTextField(value = titulo, onValueChange = { titulo = it },
                    label = { Text("Título") }, singleLine = true,
                    modifier = Modifier.fillMaxWidth(), shape = AppShapes.small)
                OutlinedTextField(value = descricao, onValueChange = { descricao = it },
                    label = { Text("Descrição") },
                    modifier = Modifier.fillMaxWidth().height(90.dp), shape = AppShapes.small)
                OutlinedTextField(value = responsavel, onValueChange = { responsavel = it },
                    label = { Text("Responsável") }, singleLine = true,
                    modifier = Modifier.fillMaxWidth(), shape = AppShapes.small)

                ExposedDropdownMenuBox(
                    expanded = dropdownExpanded,
                    onExpandedChange = { dropdownExpanded = !dropdownExpanded }
                ) {
                    OutlinedTextField(
                        value = etapaSelecionada, onValueChange = {}, readOnly = true,
                        label = { Text("Etapa") },
                        trailingIcon = { Icon(Icons.Default.ArrowDropDown, null) },
                        modifier = Modifier.menuAnchor().fillMaxWidth(), shape = AppShapes.small
                    )
                    ExposedDropdownMenu(
                        expanded = dropdownExpanded,
                        onDismissRequest = { dropdownExpanded = false }
                    ) {
                        etapas.forEach { etapa ->
                            DropdownMenuItem(
                                text = { Text(etapa) },
                                onClick = { etapaSelecionada = etapa; dropdownExpanded = false }
                            )
                        }
                    }
                }

                OutlinedTextField(value = investimento, onValueChange = { investimento = it },
                    label = { Text("Investimento (R$)") }, singleLine = true,
                    modifier = Modifier.fillMaxWidth(), shape = AppShapes.small)
                OutlinedTextField(value = retorno, onValueChange = { retorno = it },
                    label = { Text("Retorno Esperado (R$)") }, singleLine = true,
                    modifier = Modifier.fillMaxWidth(), shape = AppShapes.small)
                OutlinedTextField(value = prazo, onValueChange = { prazo = it },
                    label = { Text("Prazo (Ex: Jun/2025)") }, singleLine = true,
                    modifier = Modifier.fillMaxWidth(), shape = AppShapes.small)
            }
        },
        confirmButton = {
            Button(
                onClick = {
                    if (titulo.isNotBlank() && responsavel.isNotBlank()) {
                        onSave(Projeto(System.currentTimeMillis().toString(),
                            titulo, descricao, responsavel, etapaSelecionada,
                            "No Prazo", "R$ $investimento", "R$ $retorno", prazo))
                    }
                },
                colors = ButtonDefaults.buttonColors(containerColor = PrimaryBlue)
            ) { Text("Salvar") }
        },
        dismissButton = {
            TextButton(onClick = onDismiss) { Text("Cancelar", color = TextSecondary) }
        }
    )
}

@Preview(showBackground = true)
@Composable
fun PreviewProjetosGestor() {
    AguiaBrancaTheme { ProjetosScreen(isLideranca = false) }
}

@Preview(showBackground = true)
@Composable
fun PreviewProjetosLideranca() {
    AguiaBrancaTheme { ProjetosScreen(isLideranca = true) }
}
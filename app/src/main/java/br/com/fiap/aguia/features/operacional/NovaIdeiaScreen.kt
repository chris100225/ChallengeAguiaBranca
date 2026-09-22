package br.com.fiap.aguia.features.operacional

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.KeyboardType
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import br.com.fiap.aguia.ui.theme.*

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun NovaIdeiaScreen(
    onNavigateBack: () -> Unit,
    onSaveIdeia: (titulo: String, descricao: String, area: String, temGanho: Boolean, valorEstimado: String) -> Unit
) {
    var titulo by remember { mutableStateOf("") }
    var descricao by remember { mutableStateOf("") }
    var areaSelecionada by remember { mutableStateOf("") }
    var dropdownExpandido by remember { mutableStateOf(false) }
    val opcoesArea = listOf("Logística", "Recursos Humanos (RH)", "Manutenção", "Tecnologia (TI)", "Operações", "Comercial")
    var trazGanhoFinanceiro by remember { mutableStateOf(false) }
    var valorEstimado by remember { mutableStateOf("") }

    val isFormValid = titulo.isNotBlank() &&
            descricao.isNotBlank() &&
            areaSelecionada.isNotBlank() &&
            (!trazGanhoFinanceiro || valorEstimado.isNotBlank())

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Nova Ideia", color = SurfaceWhite) },
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
        bottomBar = {
            Surface(
                color = BackgroundLight,
                shadowElevation = 8.dp
            ) {
                Box(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(16.dp)
                ) {
                    Button(
                        onClick = { onSaveIdeia(titulo, descricao, areaSelecionada, trazGanhoFinanceiro, valorEstimado) },
                        modifier = Modifier.fillMaxWidth().height(50.dp),
                        enabled = isFormValid,
                        shape = AppShapes.medium,
                        colors = ButtonDefaults.buttonColors(containerColor = SecondaryBlue)
                    ) {
                        Text(
                            text = "Enviar Ideia",
                            style = AppTypography.bodyLarge.copy(color = SurfaceWhite)
                        )
                    }
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

            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .verticalScroll(rememberScrollState())
                    .padding(16.dp)
            ) {
                Text(
                    text = "Qual é a sua ideia para melhorar o dia a dia?",
                    style = AppTypography.titleLarge,
                    modifier = Modifier.padding(bottom = 24.dp)
                )

                OutlinedTextField(
                    value = titulo,
                    onValueChange = { titulo = it },
                    label = { Text("Título da Ideia") },
                    placeholder = { Text("Ex: Melhoria na Iluminação") },
                    modifier = Modifier.fillMaxWidth(),
                    singleLine = true,
                    shape = AppShapes.small
                )

                Spacer(modifier = Modifier.height(16.dp))

                ExposedDropdownMenuBox(
                    expanded = dropdownExpandido,
                    onExpandedChange = { dropdownExpandido = !dropdownExpandido }
                ) {
                    OutlinedTextField(
                        value = areaSelecionada,
                        onValueChange = {},
                        readOnly = true,
                        label = { Text("Área de Aplicação") },
                        placeholder = { Text("Selecione o departamento...") },
                        trailingIcon = { ExposedDropdownMenuDefaults.TrailingIcon(expanded = dropdownExpandido) },
                        modifier = Modifier.menuAnchor().fillMaxWidth(),
                        shape = AppShapes.small
                    )
                    ExposedDropdownMenu(
                        expanded = dropdownExpandido,
                        onDismissRequest = { dropdownExpandido = false }
                    ) {
                        opcoesArea.forEach { opcao ->
                            DropdownMenuItem(
                                text = { Text(opcao) },
                                onClick = {
                                    areaSelecionada = opcao
                                    dropdownExpandido = false
                                }
                            )
                        }
                    }
                }

                Spacer(modifier = Modifier.height(16.dp))

                OutlinedTextField(
                    value = descricao,
                    onValueChange = { descricao = it },
                    label = { Text("Descrição detalhada") },
                    placeholder = { Text("Descreva o problema atual e a solução que sugere...") },
                    modifier = Modifier.fillMaxWidth().height(150.dp),
                    maxLines = 5,
                    shape = AppShapes.small
                )

                Spacer(modifier = Modifier.height(16.dp))

                Card(
                    colors = CardDefaults.cardColors(containerColor = SurfaceWhite),
                    shape = AppShapes.medium,
                    elevation = CardDefaults.cardElevation(defaultElevation = 1.dp),
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Column(modifier = Modifier.padding(16.dp)) {
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            verticalAlignment = Alignment.CenterVertically,
                            horizontalArrangement = Arrangement.SpaceBetween
                        ) {
                            Text(
                                text = "A ideia traz redução de custo ou ganho financeiro?",
                                style = AppTypography.bodyLarge,
                                modifier = Modifier.weight(1f).padding(end = 16.dp)
                            )
                            Switch(
                                checked = trazGanhoFinanceiro,
                                onCheckedChange = {
                                    trazGanhoFinanceiro = it
                                    if (!it) valorEstimado = ""
                                },
                                colors = SwitchDefaults.colors(
                                    checkedThumbColor = PrimaryBlue,
                                    checkedTrackColor = SecondaryBlue.copy(alpha = 0.5f)
                                )
                            )
                        }

                        if (trazGanhoFinanceiro) {
                            Spacer(modifier = Modifier.height(12.dp))
                            OutlinedTextField(
                                value = valorEstimado,
                                onValueChange = { valorEstimado = it },
                                label = { Text("Valor Estimado (R$)") },
                                placeholder = { Text("Ex: 1500.00") },
                                keyboardOptions = KeyboardOptions(keyboardType = KeyboardType.Number),
                                modifier = Modifier.fillMaxWidth(),
                                singleLine = true,
                                shape = AppShapes.small,
                                leadingIcon = { Text("R$", modifier = Modifier.padding(start = 12.dp), color = TextSecondary) }
                            )
                        }
                    }
                }

                Spacer(modifier = Modifier.height(16.dp))
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun PreviewNovaIdeiaScreen() {
    AguiaBrancaTheme {
        NovaIdeiaScreen(
            onNavigateBack = {},
            onSaveIdeia = { _, _, _, _, _ -> }
        )
    }
}
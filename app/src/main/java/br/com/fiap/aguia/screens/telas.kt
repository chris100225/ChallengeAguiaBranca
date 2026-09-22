package br.com.fiap.aguia.screens

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.CheckCircle
import androidx.compose.material.icons.filled.Star
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

// --- MOCK DE DADOS (Substituir pelo Firebase Firestore depois) ---
data class Ideia(val titulo: String, val status: String, val autor: String)

val ideiasMock = listOf(
    Ideia("App V1 Motoristas", "Aprovada", "Hugo"),
    Ideia("IA para Precificação", "Em Análise", "Ana"),
    Ideia("Redução de Papel na Logística", "Pendente", "Carlos")
)

// ============================================================================
// 1. TELA PERFIL OPERACIONAL (Foco: Cadastrar ideias e Gamificação)
// ============================================================================
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun OperacionalScreen() {
    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Minhas Ideias", fontWeight = FontWeight.Bold) },
                actions = {
                    // Gamificação Badge
                    Row(
                        verticalAlignment = Alignment.CenterVertically,
                        modifier = Modifier
                            .padding(end = 16.dp)
                            .background(Color(0xFFFFF3CD), RoundedCornerShape(12.dp))
                            .padding(horizontal = 8.dp, vertical = 4.dp)
                    ) {
                        Icon(Icons.Filled.Star, contentDescription = "Pontos", tint = Color(0xFFFFC107), modifier = Modifier.size(16.dp))
                        Spacer(modifier = Modifier.width(4.dp))
                        Text("450 XP", color = Color(0xFF856404), fontWeight = FontWeight.Bold, fontSize = 14.sp)
                    }
                }
            )
        },
        floatingActionButton = {
            FloatingActionButton(
                onClick = { /* TODO: Abrir modal de nova ideia */ },
                containerColor = Color(0xFF004B87) // Azul Águia Branca
            ) {
                Icon(Icons.Filled.Add, contentDescription = "Nova Ideia", tint = Color.White)
            }
        }
    ) { paddingValues ->
        LazyColumn(
            contentPadding = paddingValues,
            modifier = Modifier.padding(horizontal = 16.dp).fillMaxSize()
        ) {
            item {
                Text(
                    "Acompanhe suas sugestões",
                    style = MaterialTheme.typography.titleMedium,
                    modifier = Modifier.padding(vertical = 16.dp),
                    color = Color.Gray
                )
            }
            items(ideiasMock) { ideia ->
                IdeiaCard(ideia)
            }
        }
    }
}

// ============================================================================
// 2. TELA PERFIL GESTOR (Foco: Triagem e Kanban)
// ============================================================================
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun GestorScreen() {
    var selectedTabIndex by remember { mutableIntStateOf(0) }
    val tabs = listOf("Novas", "Em Análise", "Projetos Ativos")

    Scaffold(
        topBar = {
            TopAppBar(title = { Text("Painel do Gestor (Triagem)", fontWeight = FontWeight.Bold) })
        }
    ) { paddingValues ->
        Column(modifier = Modifier.padding(paddingValues).fillMaxSize()) {
            TabRow(selectedTabIndex = selectedTabIndex) {
                tabs.forEachIndexed { index, title ->
                    Tab(
                        selected = selectedTabIndex == index,
                        onClick = { selectedTabIndex = index },
                        text = { Text(title) }
                    )
                }
            }

            // Simula as colunas do Kanban
            LazyColumn(modifier = Modifier.padding(16.dp).fillMaxSize()) {
                val ideiasFiltradas = when(selectedTabIndex) {
                    0 -> ideiasMock.filter { it.status == "Pendente" }
                    1 -> ideiasMock.filter { it.status == "Em Análise" }
                    else -> ideiasMock.filter { it.status == "Aprovada" }
                }

                items(ideiasFiltradas) { ideia ->
                    IdeiaCard(ideia)
                }
            }
        }
    }
}

// ============================================================================
// 3. TELA PERFIL LIDERANÇA (Foco: Dashboard de Indicadores)
// ============================================================================
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun LiderancaScreen() {
    Scaffold(
        topBar = {
            TopAppBar(title = { Text("Dashboard Estratégico", fontWeight = FontWeight.Bold) })
        }
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .padding(paddingValues)
                .padding(16.dp)
                .fillMaxSize()
        ) {
            Text("Visão Geral - Q3 2026", style = MaterialTheme.typography.titleLarge, modifier = Modifier.padding(bottom = 16.dp))

            Row(modifier = Modifier.fillMaxWidth(), horizontalArrangement = Arrangement.spacedBy(16.dp)) {
                DashboardCard(
                    title = "Ideias Geradas",
                    value = "142",
                    modifier = Modifier.weight(1f)
                )
                DashboardCard(
                    title = "ROI Estimado",
                    value = "R$ 1.2M",
                    modifier = Modifier.weight(1f),
                    valueColor = Color(0xFF28A745)
                )
            }

            Spacer(modifier = Modifier.height(24.dp))
            Text("Orientações Estratégicas", style = MaterialTheme.typography.titleMedium, modifier = Modifier.padding(bottom = 8.dp))

            // Lista de Desafios Open Innovation / Internos
            Card(modifier = Modifier.fillMaxWidth(), colors = CardDefaults.cardColors(containerColor = Color(0xFFF8F9FA))) {
                Column(modifier = Modifier.padding(16.dp)) {
                    Text("• Redução de Custo na Frota", fontWeight = FontWeight.Medium)
                    Spacer(modifier = Modifier.height(4.dp))
                    Text("• Retenção de Talentos (Comércio)", fontWeight = FontWeight.Medium)
                }
            }
        }
    }
}

// ============================================================================
// COMPONENTES REUTILIZÁVEIS
// ============================================================================
@Composable
fun IdeiaCard(ideia: Ideia) {
    Card(
        modifier = Modifier
            .fillMaxWidth()
            .padding(bottom = 12.dp),
        elevation = CardDefaults.cardElevation(defaultElevation = 2.dp),
        colors = CardDefaults.cardColors(containerColor = Color.White)
    ) {
        Column(modifier = Modifier.padding(16.dp)) {
            Text(ideia.titulo, fontSize = 18.sp, fontWeight = FontWeight.Bold)
            Spacer(modifier = Modifier.height(8.dp))
            Row(verticalAlignment = Alignment.CenterVertically, horizontalArrangement = Arrangement.SpaceBetween, modifier = Modifier.fillMaxWidth()) {
                Text("Autor: ${ideia.autor}", color = Color.Gray, fontSize = 14.sp)
                StatusTag(ideia.status)
            }
        }
    }
}

@Composable
fun StatusTag(status: String) {
    val (bgColor, textColor) = when (status) {
        "Aprovada" -> Pair(Color(0xFFD4EDDA), Color(0xFF155724))
        "Em Análise" -> Pair(Color(0xFFFFF3CD), Color(0xFF856404))
        else -> Pair(Color(0xFFE2E3E5), Color(0xFF383D41))
    }

    Surface(
        color = bgColor,
        shape = RoundedCornerShape(16.dp)
    ) {
        Text(
            text = status,
            color = textColor,
            fontSize = 12.sp,
            fontWeight = FontWeight.SemiBold,
            modifier = Modifier.padding(horizontal = 8.dp, vertical = 4.dp)
        )
    }
}

@Composable
fun DashboardCard(title: String, value: String, modifier: Modifier = Modifier, valueColor: Color = Color.Black) {
    Card(
        modifier = modifier.height(100.dp),
        colors = CardDefaults.cardColors(containerColor = Color.White),
        elevation = CardDefaults.cardElevation(defaultElevation = 4.dp)
    ) {
        Column(
            modifier = Modifier.padding(16.dp).fillMaxSize(),
            verticalArrangement = Arrangement.Center
        ) {
            Text(title, fontSize = 14.sp, color = Color.Gray)
            Text(value, fontSize = 24.sp, fontWeight = FontWeight.Bold, color = valueColor)
        }
    }
}

// Previews para o Android Studio
@Preview(showBackground = true)
@Composable
fun PreviewOperacional() { MaterialTheme { OperacionalScreen() } }

@Preview(showBackground = true)
@Composable
fun PreviewGestor() { MaterialTheme { GestorScreen() } }
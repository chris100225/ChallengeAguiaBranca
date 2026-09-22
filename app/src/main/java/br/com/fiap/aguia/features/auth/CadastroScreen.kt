package br.com.fiap.aguia.features.auth

import androidx.compose.foundation.layout.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.ArrowBack
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.unit.dp
import br.com.fiap.aguia.data.FirebaseRepository
import br.com.fiap.aguia.ui.theme.*
import kotlinx.coroutines.launch

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun CadastroScreen(
    onNavigateBack: () -> Unit,
    onCadastroSuccess: () -> Unit
) {
    var nome by remember { mutableStateOf("") }
    var matricula by remember { mutableStateOf("") }
    var email by remember { mutableStateOf("") }
    var senha by remember { mutableStateOf("") }
    var isLoading by remember { mutableStateOf(false) }
    var errorMessage by remember { mutableStateOf("") }

    val repository = remember { FirebaseRepository() }
    val scope = rememberCoroutineScope()

    Scaffold(
        topBar = {
            TopAppBar(
                title = { Text("Criar Conta", color = SurfaceWhite) },
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
        containerColor = BackgroundLight
    ) { paddingValues ->
        Column(
            modifier = Modifier
                .fillMaxSize()
                .padding(paddingValues)
                .padding(24.dp),
            verticalArrangement = Arrangement.spacedBy(16.dp)
        ) {
            Text(
                text = "Preencha seus dados para acessar o Portal de Inovação.",
                style = AppTypography.bodyLarge,
                color = TextSecondary,
                modifier = Modifier.padding(bottom = 8.dp)
            )

            OutlinedTextField(
                value = nome,
                onValueChange = { nome = it },
                label = { Text("Nome Completo") },
                modifier = Modifier.fillMaxWidth(),
                singleLine = true,
                shape = AppShapes.small
            )

            OutlinedTextField(
                value = matricula,
                onValueChange = { matricula = it },
                label = { Text("Matrícula") },
                modifier = Modifier.fillMaxWidth(),
                singleLine = true,
                shape = AppShapes.small
            )

            OutlinedTextField(
                value = email,
                onValueChange = { email = it },
                label = { Text("E-mail") },
                modifier = Modifier.fillMaxWidth(),
                singleLine = true,
                shape = AppShapes.small
            )

            OutlinedTextField(
                value = senha,
                onValueChange = { senha = it },
                label = { Text("Senha") },
                visualTransformation = PasswordVisualTransformation(),
                modifier = Modifier.fillMaxWidth(),
                singleLine = true,
                shape = AppShapes.small
            )

            if (errorMessage.isNotBlank()) {
                Text(
                    text = errorMessage,
                    color = ErrorRed,
                    style = AppTypography.bodyLarge
                )
            }

            Spacer(modifier = Modifier.weight(1f))

            Button(
                onClick = {
                    isLoading = true
                    errorMessage = ""
                    scope.launch {
                        val resultado = repository.cadastrar(nome, matricula, email, senha)
                        isLoading = false
                        resultado.fold(
                            onSuccess = { onCadastroSuccess() },
                            onFailure = { errorMessage = "Erro ao cadastrar. Verifique os dados." }
                        )
                    }
                },
                modifier = Modifier
                    .fillMaxWidth()
                    .height(50.dp),
                enabled = nome.isNotBlank() && matricula.isNotBlank() &&
                        email.isNotBlank() && senha.isNotBlank() && !isLoading,
                shape = AppShapes.medium,
                colors = ButtonDefaults.buttonColors(containerColor = SecondaryBlue)
            ) {
                if (isLoading) {
                    CircularProgressIndicator(
                        modifier = Modifier.size(24.dp),
                        color = SurfaceWhite
                    )
                } else {
                    Text(
                        "Cadastrar",
                        style = AppTypography.bodyLarge.copy(
                            color = SurfaceWhite,
                            fontWeight = FontWeight.Bold
                        )
                    )
                }
            }
        }
    }
}
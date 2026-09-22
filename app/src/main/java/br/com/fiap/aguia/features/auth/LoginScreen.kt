package br.com.fiap.aguia.features.auth

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.DirectionsBus
import androidx.compose.material.icons.filled.Email
import androidx.compose.material.icons.filled.Lock
import androidx.compose.material.icons.filled.Visibility
import androidx.compose.material.icons.filled.VisibilityOff
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.input.VisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import br.com.fiap.aguia.data.BackendRepository
import br.com.fiap.aguia.ui.theme.*
import kotlinx.coroutines.launch

// Traduz o perfil que vem do backend (.NET) para o formato que a navegação
// do app já espera (mesmo formato que o Firebase usava).
private fun perfilParaRota(perfilBackend: String): String {
    return when (perfilBackend.lowercase()) {
        "operador" -> "operacional"
        "gestor" -> "gestor"
        "lider" -> "lideranca"
        else -> perfilBackend.lowercase()
    }
}

@Composable
fun LoginScreen(
    onLoginSuccess: (String) -> Unit,
    onNavigateToCadastro: () -> Unit
) {
    var email by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var isLoading by remember { mutableStateOf(false) }
    var errorMessage by remember { mutableStateOf("") }
    var passwordVisible by remember { mutableStateOf(false) }

    val context = LocalContext.current
    val repository = remember { BackendRepository(context) }
    val scope = rememberCoroutineScope()

    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(DarkBlue)
    ) {
        // Círculos decorativos de fundo
        Box(
            modifier = Modifier
                .size(220.dp)
                .align(Alignment.TopEnd)
                .offset(x = 60.dp, y = (-60).dp)
                .clip(CircleShape)
                .background(PrimaryYellow.copy(alpha = 0.12f))
        )
        Box(
            modifier = Modifier
                .size(160.dp)
                .align(Alignment.TopStart)
                .offset(x = (-40).dp, y = 80.dp)
                .clip(CircleShape)
                .background(Color.White.copy(alpha = 0.05f))
        )
        Box(
            modifier = Modifier
                .size(100.dp)
                .align(Alignment.CenterEnd)
                .offset(x = 20.dp, y = (-80).dp)
                .clip(CircleShape)
                .background(PrimaryYellow.copy(alpha = 0.08f))
        )

        // Área superior: ícone e nome da marca
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(top = 80.dp),
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Box(
                modifier = Modifier
                    .size(72.dp)
                    .clip(CircleShape)
                    .background(PrimaryYellow.copy(alpha = 0.18f))
                    .border(2.dp, PrimaryYellow.copy(alpha = 0.4f), CircleShape),
                contentAlignment = Alignment.Center
            ) {
                Icon(
                    imageVector = Icons.Default.DirectionsBus,
                    contentDescription = "Logo Águia Branca",
                    tint = PrimaryYellow,
                    modifier = Modifier.size(36.dp)
                )
            }

            Spacer(modifier = Modifier.height(12.dp))

            Text(
                text = "Águia Branca",
                style = AppTypography.headlineMedium,
                color = PrimaryYellow
            )
            Text(
                text = "Portal de Inovação",
                style = AppTypography.bodyLarge,
                color = Color.White.copy(alpha = 0.65f)
            )
        }

        // Card branco na parte inferior
        Column(
            modifier = Modifier
                .align(Alignment.BottomCenter)
                .fillMaxWidth()
                .clip(RoundedCornerShape(topStart = 28.dp, topEnd = 28.dp))
                .background(SurfaceWhite)
                .padding(horizontal = 24.dp, vertical = 28.dp)
        ) {
            Text(
                text = "Bem-vindo de volta",
                style = AppTypography.titleMedium,
                color = TextPrimary,
                fontWeight = FontWeight.SemiBold
            )

            Spacer(modifier = Modifier.height(20.dp))

            OutlinedTextField(
                value = email,
                onValueChange = { email = it },
                label = { Text("E-mail corporativo") },
                leadingIcon = {
                    Icon(
                        imageVector = Icons.Default.Email,
                        contentDescription = null,
                        tint = PrimaryBlue
                    )
                },
                shape = AppShapes.small,
                modifier = Modifier.fillMaxWidth()
            )

            Spacer(modifier = Modifier.height(12.dp))

            OutlinedTextField(
                value = password,
                onValueChange = { password = it },
                label = { Text("Senha") },
                leadingIcon = {
                    Icon(
                        imageVector = Icons.Default.Lock,
                        contentDescription = null,
                        tint = PrimaryBlue
                    )
                },
                trailingIcon = {
                    IconButton(onClick = { passwordVisible = !passwordVisible }) {
                        Icon(
                            imageVector = if (passwordVisible)
                                Icons.Default.VisibilityOff
                            else
                                Icons.Default.Visibility,
                            contentDescription = if (passwordVisible)
                                "Ocultar senha"
                            else
                                "Mostrar senha"
                        )
                    }
                },
                visualTransformation = if (passwordVisible)
                    VisualTransformation.None
                else
                    PasswordVisualTransformation(),
                shape = AppShapes.small,
                modifier = Modifier.fillMaxWidth()
            )

            if (errorMessage.isNotBlank()) {
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = errorMessage,
                    color = ErrorRed,
                    style = AppTypography.bodySmall
                )
            }

            Spacer(modifier = Modifier.height(24.dp))

            Button(
                onClick = {
                    isLoading = true
                    errorMessage = ""
                    scope.launch {
                        val resultado = repository.login(email, password)
                        isLoading = false
                        resultado.fold(
                            onSuccess = { perfilBackend -> onLoginSuccess(perfilParaRota(perfilBackend)) },
                            onFailure = { errorMessage = "E-mail ou senha incorretos." }
                        )
                    }
                },
                modifier = Modifier
                    .fillMaxWidth()
                    .height(50.dp),
                enabled = email.isNotBlank() && password.isNotBlank() && !isLoading,
                shape = AppShapes.medium,
                colors = ButtonDefaults.buttonColors(containerColor = PrimaryBlue)
            ) {
                if (isLoading) {
                    CircularProgressIndicator(
                        modifier = Modifier.size(24.dp),
                        color = SurfaceWhite
                    )
                } else {
                    Text(
                        "Entrar",
                        style = AppTypography.bodyLarge.copy(
                            color = SurfaceWhite,
                            fontWeight = FontWeight.Bold
                        )
                    )
                }
            }

            Spacer(modifier = Modifier.height(12.dp))

            TextButton(
                onClick = onNavigateToCadastro,
                modifier = Modifier.fillMaxWidth()
            ) {
                Text(
                    text = "Primeiro acesso? Crie sua conta",
                    color = SecondaryBlue,
                    style = AppTypography.bodyLarge.copy(fontWeight = FontWeight.SemiBold)
                )
            }
        }
    }
}

@Preview
@Composable
fun LoginScreenPreview() {
    AguiaBrancaTheme {
        LoginScreen(
            onLoginSuccess = {},
            onNavigateToCadastro = {}
        )
    }
}
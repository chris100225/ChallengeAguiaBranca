package br.com.fiap.aguia

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.ui.Modifier
import br.com.fiap.aguia.navigation.AppNavigation
import br.com.fiap.aguia.ui.theme.AguiaBrancaTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            // Usa o tema que configuramos lá no início
            AguiaBrancaTheme {
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    // Chama a navegação principal (que está no arquivo AppNavigation.kt da pasta navigation)
                    AppNavigation()
                }
            }
        }
    }
}
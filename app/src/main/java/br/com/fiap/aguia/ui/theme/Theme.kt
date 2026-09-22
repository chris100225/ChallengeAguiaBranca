package br.com.fiap.aguia.ui.theme

import android.app.Activity
import androidx.compose.foundation.isSystemInDarkTheme
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.lightColorScheme
import androidx.compose.material3.darkColorScheme
import androidx.compose.runtime.Composable
import androidx.compose.runtime.SideEffect
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.toArgb
import androidx.compose.ui.platform.LocalView
import androidx.core.view.WindowCompat

private val LightColorScheme = lightColorScheme(
    primary = PrimaryBlue,
    secondary = SecondaryBlue,
    background = BackgroundLight,
    surface = SurfaceWhite,
    onPrimary = SurfaceWhite,
    onBackground = TextPrimary,
    onSurface = TextPrimary,
    error = ErrorRed
)

// Se quiser suporte a Dark Mode depois, basta preencher aqui com cores escuras
private val DarkColorScheme = darkColorScheme(
    primary = SecondaryBlue,
    secondary = PrimaryBlue,
    background = TextPrimary,
    surface = Color(0xFF343A40),
    onPrimary = SurfaceWhite,
    onBackground = BackgroundLight,
    onSurface = BackgroundLight,
    error = ErrorRed
)

@Composable
fun AguiaBrancaTheme(
    darkTheme: Boolean = isSystemInDarkTheme(),
    content: @Composable () -> Unit
) {
    val colorScheme = if (darkTheme) DarkColorScheme else LightColorScheme
    val view = LocalView.current

    if (!view.isInEditMode) {
        SideEffect {
            val window = (view.context as Activity).window
            window.statusBarColor = colorScheme.primary.toArgb()
            WindowCompat.getInsetsController(window, view).isAppearanceLightStatusBars = darkTheme
        }
    }

    MaterialTheme(
        colorScheme = colorScheme,
        typography = AppTypography,
        shapes = AppShapes,
        content = content
    )
}
package br.com.fiap.aguia.ui.theme

import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Shapes
import androidx.compose.ui.unit.dp

val AppShapes = Shapes(
    small = RoundedCornerShape(4.dp),  // Para botões pequenos ou tags
    medium = RoundedCornerShape(12.dp), // Padrão para Cards de ideias e modais
    large = RoundedCornerShape(24.dp)   // Para elementos grandes, como bottom sheets
)
package br.com.fiap.aguia.data.network

import retrofit2.Response
import retrofit2.http.*

// ===================== DTOs =====================

data class LoginRequest(val email: String, val senha: String)
data class LoginResponse(
    val token: String,
    val usuarioId: String,
    val nome: String,
    val email: String,
    val perfil: String,
    val expiraEm: String
)

data class UsuarioDTO(
    val nome: String,
    val email: String,
    val senha: String,
    val perfil: String // "Operador" | "Gestor" | "Lider"
)
data class UsuarioRespostaDTO(
    val id: String,
    val nome: String,
    val email: String,
    val perfil: String,
    val ativo: Boolean,
    val dataCadastro: String
)

data class EstrategiaDTO(
    val campanha: String,
    val categoria: String,
    val descricao: String,
    val dataInicio: String? = null,
    val dataFim: String? = null,
    val ativa: Boolean = true
)
data class EstrategiaRespostaDTO(
    val id: String,
    val campanha: String,
    val categoria: String,
    val descricao: String,
    val data: String,
    val dataInicio: String,
    val dataFim: String,
    val ativa: Boolean,
    val criadoPorUsuarioId: String,
    val dataCriacao: String,
    val dataAtualizacao: String?
)

data class IdeiaInovacaoDTO(
    val titulo: String,
    val descricao: String,
    val problemaEnfrentado: String,
    val impactoEsperado: String,
    val estrategiaId: String?
)
data class IdeiaInovacaoRespostaDTO(
    val id: String,
    val titulo: String,
    val descricao: String,
    val problemaEnfrentado: String,
    val impactoEsperado: String,
    val status: String,
    val prioridade: String?,
    val estrategiaId: String?,
    val campanhaEstrategia: String?,
    val operadorId: String,
    val nomeOperador: String,
    val feedbackGestor: String?,
    val avaliadoPorGestorId: String?,
    val dataAvaliacao: String?,
    val dataCriacao: String,
    val dataAtualizacao: String?
)
data class AvaliarIdeiaDTO(val status: String, val feedbackGestor: String? = null, val prioridade: String? = null)

data class ProjetoDTO(
    val nome: String,
    val descricao: String,
    val estrategiaId: String?,
    val ideiaInovacaoId: String?,
    val investimentoPrevisto: Double,
    val retornoFinanceiroEsperado: Double,
    val dataInicio: String,
    val prazoFinal: String
)
data class ProjetoRespostaDTO(
    val id: String,
    val nome: String,
    val descricao: String,
    val estrategiaId: String?,
    val campanhaEstrategia: String?,
    val ideiaInovacaoId: String?,
    val gestorId: String,
    val nomeGestor: String,
    val etapa: String,
    val status: String,
    val investimentoPrevisto: Double,
    val investimentoReal: Double,
    val retornoFinanceiroEsperado: Double,
    val retornoFinanceiroReal: Double,
    val lucroObtido: Double,
    val roiPercentual: Double,
    val aumentoProdutividadePercentual: Double,
    val dataInicio: String,
    val prazoFinal: String,
    val dataConclusao: String?,
    val estaAtrasado: Boolean,
    val dataCriacao: String,
    val dataAtualizacao: String?
)

// ===================== Endpoints =====================

interface ApiService {

    @POST("api/Autenticacao/login")
    suspend fun login(@Body request: LoginRequest): Response<LoginResponse>

    // --- Usuario ---
    @POST("api/Usuario")
    suspend fun cadastrarUsuario(@Body usuario: UsuarioDTO): Response<UsuarioRespostaDTO>

    @GET("api/Usuario/meu-perfil")
    suspend fun meuPerfil(): Response<UsuarioRespostaDTO>

    @GET("api/Usuario")
    suspend fun listarUsuarios(): Response<List<UsuarioRespostaDTO>>

    // --- Estrategia ---
    @GET("api/Estrategia")
    suspend fun listarEstrategias(): Response<List<EstrategiaRespostaDTO>>

    @GET("api/Estrategia/vigente")
    suspend fun estrategiaVigente(): Response<EstrategiaRespostaDTO>

    @POST("api/Estrategia")
    suspend fun criarEstrategia(@Body dto: EstrategiaDTO): Response<EstrategiaRespostaDTO>

    // --- IdeiaInovacao ---
    @GET("api/IdeiaInovacao")
    suspend fun listarIdeias(): Response<List<IdeiaInovacaoRespostaDTO>>

    @GET("api/IdeiaInovacao/minhas")
    suspend fun minhasIdeias(): Response<List<IdeiaInovacaoRespostaDTO>>

    @POST("api/IdeiaInovacao")
    suspend fun criarIdeia(@Body dto: IdeiaInovacaoDTO): Response<IdeiaInovacaoRespostaDTO>

    @PATCH("api/IdeiaInovacao/{id}/avaliar")
    suspend fun avaliarIdeia(@Path("id") id: String, @Body dto: AvaliarIdeiaDTO): Response<IdeiaInovacaoRespostaDTO>

    // --- Projeto ---
    @GET("api/Projeto")
    suspend fun listarProjetos(): Response<List<ProjetoRespostaDTO>>

    @POST("api/Projeto")
    suspend fun criarProjeto(@Body dto: ProjetoDTO): Response<ProjetoRespostaDTO>

    @GET("api/Projeto/por-estrategia/{estrategiaId}")
    suspend fun projetosPorEstrategia(@Path("estrategiaId") estrategiaId: String): Response<List<ProjetoRespostaDTO>>
}
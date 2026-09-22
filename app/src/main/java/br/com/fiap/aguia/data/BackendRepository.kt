package br.com.fiap.aguia.data

import android.content.Context
import br.com.fiap.aguia.data.network.*

/**
 * Substitui o FirebaseRepository.kt (Sprint 1) pela nova API .NET + MongoDB (Sprint 2).
 * Mantém nomes de método parecidos para facilitar a troca nas telas/ViewModels,
 * mas ATENÇÃO: alguns métodos agora pedem campos novos que o Firebase não tinha
 * (ex: estrategiaId em ideias e projetos, problemaEnfrentado/impactoEsperado em vez
 * de area/valorEstimado) — as telas de cadastro precisam ser ajustadas pra coletar
 * esses campos.
 */
class BackendRepository(private val context: Context) {

    private val api: ApiService = RetrofitClient.create(context)
    private val tokenManager = TokenManager(context)

    // ==================== AUTENTICAÇÃO ====================

    /** Retorna o perfil do usuário em caso de sucesso (mesma assinatura do FirebaseRepository). */
    suspend fun login(email: String, senha: String): Result<String> {
        return try {
            val response = api.login(LoginRequest(email, senha))
            if (response.isSuccessful && response.body() != null) {
                val body = response.body()!!
                tokenManager.salvarSessao(body.token, body.usuarioId, body.nome, body.perfil)
                Result.success(body.perfil)
            } else {
                Result.failure(Exception("Credenciais inválidas (código ${response.code()})"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    /**
     * Cadastra um novo usuário. O parâmetro "matricula" do Firebase não existe mais
     * no backend novo, então foi removido daqui; "perfil" agora precisa ser
     * "Operador", "Gestor" ou "Lider" (primeira letra maiúscula).
     */
    suspend fun cadastrar(nome: String, email: String, senha: String, perfil: String = "Operador"): Result<Unit> {
        return try {
            val response = api.cadastrarUsuario(UsuarioDTO(nome, email, senha, perfil))
            if (response.isSuccessful) Result.success(Unit)
            else Result.failure(Exception("Erro ao cadastrar (código ${response.code()})"))
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    fun logout() {
        tokenManager.limpar()
    }

    fun perfilAtual(): String? = tokenManager.getPerfil()
    fun usuarioIdAtual(): String? = tokenManager.getUsuarioId()

    // ==================== IDEIAS ====================

    /**
     * Assinatura nova: em vez de (titulo, descricao, area, temGanho, valorEstimado),
     * agora é (titulo, descricao, problemaEnfrentado, impactoEsperado, estrategiaId).
     * Use estrategiaVigente() pra pegar o id da estratégia atual, se a tela não tiver isso ainda.
     */
    suspend fun salvarIdeia(
        titulo: String,
        descricao: String,
        problemaEnfrentado: String,
        impactoEsperado: String,
        estrategiaId: String?
    ): Result<Unit> = safeUnitCall {
        api.criarIdeia(IdeiaInovacaoDTO(titulo, descricao, problemaEnfrentado, impactoEsperado, estrategiaId))
    }

    suspend fun getIdeias(): Result<List<Map<String, Any?>>> = safeListCall {
        api.listarIdeias()
    }

    suspend fun getMinhasIdeias(): Result<List<Map<String, Any?>>> = safeListCall {
        api.minhasIdeias()
    }

    /** Substitui o antigo atualizarStatusIdeia — agora é "avaliar" (aprovar/rejeitar/priorizar). */
    suspend fun avaliarIdeia(ideiaId: String, novoStatus: String, feedback: String? = null): Result<Unit> {
        return try {
            val response = api.avaliarIdeia(ideiaId, AvaliarIdeiaDTO(novoStatus, feedback))
            if (response.isSuccessful) Result.success(Unit)
            else Result.failure(Exception("Erro ao avaliar ideia (código ${response.code()})"))
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    // ==================== PROJETOS ====================

    /**
     * Assinatura nova: (nome, descricao, estrategiaId, ideiaInovacaoId, investimentoPrevisto,
     * retornoFinanceiroEsperado, dataInicio, prazoFinal) — datas em formato ISO 8601
     * (ex: "2026-12-01T00:00:00Z").
     */
    suspend fun salvarProjeto(
        nome: String,
        descricao: String,
        estrategiaId: String?,
        ideiaInovacaoId: String?,
        investimentoPrevisto: Double,
        retornoFinanceiroEsperado: Double,
        dataInicio: String,
        prazoFinal: String
    ): Result<Unit> = safeUnitCall {
        api.criarProjeto(
            ProjetoDTO(nome, descricao, estrategiaId, ideiaInovacaoId, investimentoPrevisto, retornoFinanceiroEsperado, dataInicio, prazoFinal)
        )
    }

    suspend fun getProjetos(): Result<List<Map<String, Any?>>> = safeListCall {
        api.listarProjetos()
    }

    // ==================== ORIENTAÇÕES / ESTRATÉGIA ====================
    // No backend novo, "orientações" viraram "Estratégia" (com campanha/categoria vigente).

    suspend fun salvarOrientacao(campanha: String, categoria: String, descricao: String): Result<Unit> = safeUnitCall {
        api.criarEstrategia(EstrategiaDTO(campanha, categoria, descricao))
    }

    suspend fun getOrientacoes(): Result<List<Map<String, Any?>>> = safeListCall {
        api.listarEstrategias()
    }

    suspend fun getEstrategiaVigente(): Result<Map<String, Any?>?> {
        return try {
            val response = api.estrategiaVigente()
            if (response.isSuccessful) Result.success(response.body()?.toMap())
            else Result.failure(Exception("Erro ao buscar estratégia vigente (código ${response.code()})"))
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    // ==================== helpers ====================

    private suspend fun safeUnitCall(call: suspend () -> retrofit2.Response<*>): Result<Unit> {
        return try {
            val response = call()
            if (response.isSuccessful) Result.success(Unit)
            else Result.failure(Exception("Erro na API (código ${response.code()})"))
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    private suspend fun <T : Any>  safeListCall(call: suspend () -> retrofit2.Response<List<T>>): Result<List<Map<String, Any?>>> {
        return try {
            val response = call()
            if (response.isSuccessful && response.body() != null) {
                Result.success(response.body()!!.map { it.toMap() })
            } else {
                Result.failure(Exception("Erro na API (código ${response.code()})"))
            }
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}

// Converte qualquer DTO de resposta num Map<String, Any?>, pra manter compatibilidade
// com telas que hoje leem campos de um Map (como faziam com os dados do Firestore).
private fun Any.toMap(): Map<String, Any?> {
    val map = mutableMapOf<String, Any?>()
    this::class.java.declaredFields.forEach { field ->
        field.isAccessible = true
        map[field.name] = field.get(this)
    }
    return map
}
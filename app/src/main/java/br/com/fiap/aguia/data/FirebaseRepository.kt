package br.com.fiap.aguia.data

import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.firestore.FirebaseFirestore
import kotlinx.coroutines.tasks.await

class FirebaseRepository {

    private val auth = FirebaseAuth.getInstance()
    private val db = FirebaseFirestore.getInstance()

    // ==================== AUTENTICAÇÃO ====================

    suspend fun login(email: String, senha: String): Result<String> {
        return try {
            auth.signInWithEmailAndPassword(email, senha).await()
            val uid = auth.currentUser?.uid ?: return Result.failure(Exception("Usuário não encontrado"))
            val perfil = getPerfil(uid)
            Result.success(perfil)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun cadastrar(nome: String, matricula: String, email: String, senha: String, perfil: String = "operacional"): Result<Unit> {
        return try {
            val resultado = auth.createUserWithEmailAndPassword(email, senha).await()
            val uid = resultado.user?.uid ?: return Result.failure(Exception("Erro ao criar usuário"))
            db.collection("usuarios").document(uid).set(
                mapOf(
                    "nome" to nome,
                    "matricula" to matricula,
                    "email" to email,
                    "perfil" to perfil
                )
            ).await()
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    private suspend fun getPerfil(uid: String): String {
        return try {
            val doc = db.collection("usuarios").document(uid).get().await()
            doc.getString("perfil") ?: "operacional"
        } catch (e: Exception) {
            "operacional"
        }
    }

    fun logout() {
        auth.signOut()
    }

    // ==================== IDEIAS ====================

    suspend fun salvarIdeia(titulo: String, descricao: String, area: String, temGanho: Boolean, valorEstimado: String): Result<Unit> {
        return try {
            val uid = auth.currentUser?.uid ?: return Result.failure(Exception("Não autenticado"))
            db.collection("ideias").add(
                mapOf(
                    "titulo" to titulo,
                    "descricao" to descricao,
                    "area" to area,
                    "temGanho" to temGanho,
                    "valorEstimado" to valorEstimado,
                    "autorId" to uid,
                    "status" to "Pendente",
                    "criadoEm" to System.currentTimeMillis()
                )
            ).await()
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getIdeias(): Result<List<Map<String, Any>>> {
        return try {
            val snapshot = db.collection("ideias").get().await()
            val ideias = snapshot.documents.map { doc ->
                doc.data?.plus("id" to doc.id) ?: emptyMap()
            }
            Result.success(ideias)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun atualizarStatusIdeia(ideiaId: String, novoStatus: String): Result<Unit> {
        return try {
            db.collection("ideias").document(ideiaId).update("status", novoStatus).await()
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    // ==================== PROJETOS ====================

    suspend fun salvarProjeto(titulo: String, descricao: String, responsavel: String, etapa: String, investimento: String, retorno: String, prazo: String): Result<Unit> {
        return try {
            db.collection("projetos").add(
                mapOf(
                    "titulo" to titulo,
                    "descricao" to descricao,
                    "responsavel" to responsavel,
                    "etapa" to etapa,
                    "status" to "No Prazo",
                    "investimento" to investimento,
                    "retorno" to retorno,
                    "prazo" to prazo,
                    "criadoEm" to System.currentTimeMillis()
                )
            ).await()
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getProjetos(): Result<List<Map<String, Any>>> {
        return try {
            val snapshot = db.collection("projetos").get().await()
            val projetos = snapshot.documents.map { doc ->
                doc.data?.plus("id" to doc.id) ?: emptyMap()
            }
            Result.success(projetos)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    // ==================== ORIENTAÇÕES ====================

    suspend fun salvarOrientacao(titulo: String, descricao: String): Result<Unit> {
        return try {
            db.collection("orientacoes").add(
                mapOf(
                    "titulo" to titulo,
                    "descricao" to descricao,
                    "criadoEm" to System.currentTimeMillis()
                )
            ).await()
            Result.success(Unit)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }

    suspend fun getOrientacoes(): Result<List<Map<String, Any>>> {
        return try {
            val snapshot = db.collection("orientacoes").get().await()
            val orientacoes = snapshot.documents.map { doc ->
                doc.data?.plus("id" to doc.id) ?: emptyMap()
            }
            Result.success(orientacoes)
        } catch (e: Exception) {
            Result.failure(e)
        }
    }
}
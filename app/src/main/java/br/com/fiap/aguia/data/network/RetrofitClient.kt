package br.com.fiap.aguia.data.network

import android.content.Context
import android.content.SharedPreferences
import okhttp3.Interceptor
import okhttp3.OkHttpClient
import okhttp3.Response
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class TokenManager(context: Context) {
    private val prefs: SharedPreferences =
        context.getSharedPreferences("aguia_auth", Context.MODE_PRIVATE)

    fun salvarSessao(token: String, usuarioId: String, nome: String, perfil: String) {
        prefs.edit()
            .putString("jwt_token", token)
            .putString("usuario_id", usuarioId)
            .putString("nome", nome)
            .putString("perfil", perfil)
            .apply()
    }

    fun getToken(): String? = prefs.getString("jwt_token", null)
    fun getPerfil(): String? = prefs.getString("perfil", null)
    fun getUsuarioId(): String? = prefs.getString("usuario_id", null)
    fun getNome(): String? = prefs.getString("nome", null)

    fun limpar() {
        prefs.edit().clear().apply()
    }
}

class AuthInterceptor(private val tokenManager: TokenManager) : Interceptor {
    override fun intercept(chain: Interceptor.Chain): Response {
        val original = chain.request()
        val token = tokenManager.getToken()
        val request = if (token != null) {
            original.newBuilder().addHeader("Authorization", "Bearer $token").build()
        } else original
        return chain.proceed(request)
    }
}

object RetrofitClient {

    // No emulador Android, 10.0.2.2 aponta pro "localhost" da sua máquina.
    // Em um celular físico na mesma rede Wi-Fi, troque pelo IP local da máquina (ex: http://192.168.0.x:5222/).
    private const val BASE_URL = "http://10.0.2.2:5222/"

    fun create(context: Context): ApiService {
        val tokenManager = TokenManager(context)

        val logging = HttpLoggingInterceptor().apply {
            level = HttpLoggingInterceptor.Level.BODY
        }

        val client = OkHttpClient.Builder()
            .addInterceptor(AuthInterceptor(tokenManager))
            .addInterceptor(logging)
            .build()

        return Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(client)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
            .create(ApiService::class.java)
    }
}
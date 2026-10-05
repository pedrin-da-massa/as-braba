import { useEffect, useState } from "react";
import type { Produto } from "./types/Produto";
import { produtoService } from "./services/produtoService";
import "./App.css";

function App() {
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState("");

  const carregarProdutos = async () => {
    try {
      setCarregando(true);
      setErro("");

      const dados = await produtoService.listar();

      setProdutos(dados);
    } catch (error) {
      console.error(error);
      setErro(
        "Não foi possível carregar os produtos. Verifique se a API está rodando."
      );
    } finally {
      setCarregando(false);
    }
  };

  useEffect(() => {
    carregarProdutos();
  }, []);

  return (
    <div className="app">
      <header className="header">
        <div>
          <h1>Minha Loja</h1>
          <p>Gerenciamento de produtos</p>
        </div>

        <button onClick={carregarProdutos} className="btn-atualizar">
          Atualizar
        </button>
      </header>

      <main className="conteudo">
        <section className="titulo-secao">
          <div>
            <h2>Produtos</h2>
            <p>Produtos cadastrados no sistema</p>
          </div>

          <span className="contador">
            {produtos.length} produto(s)
          </span>
        </section>

        {carregando && (
          <div className="mensagem">
            <p>Carregando produtos...</p>
          </div>
        )}

        {!carregando && erro && (
          <div className="mensagem erro">
            <p>{erro}</p>

            <button onClick={carregarProdutos}>
              Tentar novamente
            </button>
          </div>
        )}

        {!carregando && !erro && produtos.length === 0 && (
          <div className="mensagem">
            <p>Nenhum produto cadastrado.</p>
          </div>
        )}

        {!carregando && !erro && produtos.length > 0 && (
          <div className="produtos-grid">
            {produtos.map((produto) => (
              <article className="produto-card" key={produto.id}>
                <div className="produto-topo">
                  <span className="produto-id">
                    #{produto.id}
                  </span>

                  <span
                    className={
                      produto.ativo
                        ? "status ativo"
                        : "status inativo"
                    }
                  >
                    {produto.ativo ? "Ativo" : "Inativo"}
                  </span>
                </div>

                <h3>{produto.nome}</h3>

                <div className="produto-info">
                  <div>
                    <span>Preço</span>

                    <strong>
                      {produto.preco.toLocaleString("pt-BR", {
                        style: "currency",
                        currency: "BRL",
                      })}
                    </strong>
                  </div>

                  <div>
                    <span>Estoque</span>

                    <strong>
                      {produto.estoque} unidades
                    </strong>
                  </div>
                </div>
              </article>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}

export default App;
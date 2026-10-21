import api from "./api";
import type { Produto } from "../types/Produto";

export const produtoService = {
  listar: async (): Promise<Produto[]> => {
    const { data } = await api.get<Produto[]>("/produto");
    return data;
  },

  buscarPorId: async (id: number): Promise<Produto> => {
    const { data } = await api.get<Produto>(`/produto/${id}`);
    return data;
  },

  criar: async (
    produto: Omit<Produto, "id">
  ): Promise<Produto> => {
    const { data } = await api.post<Produto>(
      "/produto",
      produto
    );

    return data;
  },

  atualizar: async (
    id: number,
    produto: Omit<Produto, "id">
  ): Promise<Produto> => {
    const { data } = await api.put<Produto>(
      `/produto/${id}`,
      produto
    );

    return data;
  },

  excluir: async (id: number): Promise<void> => {
    await api.delete(`/produto/${id}`);
  },
};
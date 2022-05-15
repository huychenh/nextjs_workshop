import fetchJson from "../lib/fetchJson";

export default class ProductService {
    public static apiVersion = 1;
    public static ControllerUri = `api/v${ProductService.apiVersion}/products`;
  
    public static async getProducts(text = "") {
      const queryString = `text=${text}`;
      const response = await fetchJson(
        `${process.env.NEXT_PUBLIC_URL_API}/${ProductService.ControllerUri}?${queryString}`,
        { method: 'GET' }
      );

      return await response.json();
    }
}
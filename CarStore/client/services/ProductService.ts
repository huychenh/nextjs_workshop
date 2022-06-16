import fetchJson from "../lib/fetchJson";

export default class ProductService {
  public static apiVersion = 1;
  public static ControllerUri = `api/v${ProductService.apiVersion}/products`;

  public static async getProductDetail(id: any) {
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${ProductService.ControllerUri}/${id}`,
      { method: 'GET' }
    );

    return await response.json();
  }

  public static async getProducts(text = "", page = 0) {
    var queryString = `text=${text}&page=${page == 0 ? 1 : page}`;    
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${ProductService.ControllerUri}?${queryString}`,
      { method: 'GET' },
    );

    return await response.json();
  }

  public static async addProducts(accessToken: string, model: any) {
    console.log(JSON.stringify({ model }));
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${ProductService.ControllerUri}`,
      {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${accessToken}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ model }),
      }
    );

    return await response.json();
  }
}
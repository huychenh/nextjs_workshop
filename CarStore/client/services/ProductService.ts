import fetchJson from "../lib/fetchJson";

export default class ProductService {
  public static apiVersion = 1;
  public static ControllerUri = `api/v${ProductService.apiVersion}/products`;

  public static async getProducts(text = "") {
    const queryString = `text=${text}`;
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
        },
        body: JSON.stringify({ model }),
      }
    );

    return await response.json();
  }
}
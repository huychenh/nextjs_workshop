import fetchJson from "../lib/fetchJson";
export default class OrderService {
  public static apiVersion = 1;
  public static ControllerUri = `api/v${OrderService.apiVersion}/products`;

  public static async createOrder(request: any) {
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${OrderService.ControllerUri}`,
      "",
      {
        method: "POST",
        body: JSON.stringify({ model: request }),
      }
    );

    return await response.json();
  }
}
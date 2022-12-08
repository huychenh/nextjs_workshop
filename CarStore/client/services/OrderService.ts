import fetchJson from "../lib/fetchJson";
export default class OrderService {
  public static apiVersion = 1;
  public static ControllerUri = `api/v${OrderService.apiVersion}/orders`;

  public static async createOrder(accessToken: string, request: any) {
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${OrderService.ControllerUri}`,
      accessToken,
      {
        method: "POST",
        body: JSON.stringify(request),
      }
    );

    return await response.json();
  }

  public static async getOrdersByCustomerId(accessToken: string) {
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${OrderService.ControllerUri}`,
      accessToken,
    );

    return await response.json();
  }
}

import fetchJson from "../lib/fetchJson";
import { SearchModel } from "../components/Modal/Model";
export default class ProductService {
  public static apiVersion = 1;
  public static ControllerUri = `api/v${ProductService.apiVersion}/products`;

  public static async getProductDetail(id: any) {
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${ProductService.ControllerUri}/${id}`,
    );

    return await response.json();
  }

  public static async getProducts(model: SearchModel) {
    let queryString = "";
    if (model.SearchText) {
      queryString += `searchText=${model.SearchText}&`;
    }
    if (model.brand) {
      queryString += `brand=${model.brand}&`;
    }
    if (model.CategoryName) {
      queryString += `&categoryName=${model.CategoryName}&`;
    }
    queryString += `&PriceFrom=${model.PriceFrom}
    &PriceTo=${model.PriceTo}&LatestNews=${model.LatestNews}&LowestPrice=${
      model.LowestPrice
    }&page=${model.pageIndex == 0 ? 1 : model.pageIndex}
    `;
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${ProductService.ControllerUri}?${queryString}`
    );

    return await response.json();
  }

  public static async addProducts(accessToken: string, model: any) {
    console.log(JSON.stringify({ model }));
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${ProductService.ControllerUri}`,
      accessToken,
      {
        method: "POST",
        body: JSON.stringify({ model }),
      }
    );

    return await response.json();
  }
}

import fetchJson from "../lib/fetchJson";

export default class BrandService {
  public static apiVersion = 1;
  public static ControllerUri = `api/v${BrandService.apiVersion}/brands`;

  public static async getBrands(text = "") {
    const queryString = `searchText=${text}`;
    const response = await fetchJson(
      `${process.env.NEXT_PUBLIC_URL_API}/${BrandService.ControllerUri}?${queryString}`,
    );

    return await response.json();
  }
}
export interface ModalProps {
  children?: React.ReactNode;
  flag: boolean;
  title: string;
  searchBox?: string;
  handleOnOff: () => void;
  handleClear?: () => void;
  handleSearch?: () => void;
  onSearchFilter?: (model: SearchModel) => void;
}

export interface SearchModel {
  SearchText: string;
  CategoryName: string;
  PriceFrom: number;
  PriceTo?: number;
  LatestNews?: boolean;
  LowestPrice?: boolean;
  brand?: string;
  pageIndex?: number;
  pageSize: number;
}
export const modelSearchDefault: SearchModel = {
  SearchText: "",
  CategoryName: "",
  PriceFrom: 0,
  PriceTo: 0,
  LatestNews: false,
  LowestPrice: false,
  brand: "",
  pageIndex: 0,
  pageSize: 10,
};

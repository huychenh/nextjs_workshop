import { ContainerClient } from "@azure/storage-blob";
import fetchJson from "../lib/fetchJson";

export default class FileStorageService {
  public static apiVersion = 1;
  public static ControllerUri = `api/v${FileStorageService.apiVersion}/file-storage`;

  public static async getStorageContainerUrl(accessToken: string) {
    try {
      const response = await fetchJson(
        `${process.env.NEXT_PUBLIC_URL_API}/${FileStorageService.ControllerUri}/container-url`,
        accessToken,
      );
      return await response.json();
    } catch (error) {
      console.log('Failed to get storage URL.', error);
      return null;
    }
  }

  public static async uploadImages(accessToken: string, files: File[]) {
    const storageUrl = await FileStorageService.getStorageContainerUrl(accessToken);
    if (!storageUrl) {
      return [];
    }
    try {
      const fileNames = await Promise.all(files.map(file => FileStorageService.uploadBlob(storageUrl, file)));
      return fileNames.filter(x => !!x);
    } catch (error) {
      console.error('Failed to upload images', error);
      return [];
    }
  }

  public static async uploadBlob(storageUrl: string, file: File) {
    const container = new ContainerClient(storageUrl);
    const ext = file.name.substring(file.name.lastIndexOf('.'));
    const randomName = this.createUUID();
    const blobName = randomName + ext;
    const blobClient = container.getBlockBlobClient(blobName);
    const options = { blobHTTPHeaders: { blobContentType: file.type } };
    try {
      await blobClient.uploadData(file, options);
    } catch (error: any) {
      console.log('Failed to upload blob', error);
      return null;
    }
    return blobName;
  }

  private static createUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
       var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
       return v.toString(16);
    });
 }
}
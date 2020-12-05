import axios from 'axios';
import {Map} from "@/views/mars-lander/models/Map";
import {toPascal} from "@/helpers/PropertyTransformer";

export class MarsLanderApi {
  private readonly apiClient = axios.create({
    baseURL: 'http://localhost:5000'
  })

  public async GetMaps(): Promise<Map[]> {
    return await this.apiClient.get("/mars-lander/maps")
      .then(response => {
        return toPascal(response.data) as Map[];
      })
  }
}
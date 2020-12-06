import axios from 'axios';
import {Map} from "@/views/mars-lander/models/environment/Map";
import {toPascal} from "@/helpers/PropertyTransformer";
import {Generation} from "@/views/mars-lander/models/evolution/Generation";

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

  public async Land(map: Map): Promise<Generation[]> {
    return await this.apiClient.post("mars-lander/land", map)
      .then(response => {
        return toPascal(response.data) as Generation[];
      })
  }
}
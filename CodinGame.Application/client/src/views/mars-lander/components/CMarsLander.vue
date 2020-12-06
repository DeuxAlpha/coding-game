<template>
  <div class="flex flex-col items-center">
    <div class="flex flex-row items-baseline">
      <label class="mx-1" for="selected-map">Map</label>
      <select class="mx-1 border border-gray-600" id="selected-map" v-model="selectedMap" @change="onMapChange">
        <option v-for="mapElement of mapElements" :value="mapElement.Name">{{ mapElement.Name }}</option>
      </select>
      <AButton :disabled="!selectedMap" @click="requestLanding">Land</AButton>
    </div>
    <div id="map" ref="map"/>
    <div class="flex flex-row justify-between" v-show="selectedGeneration > 0">
      <AButton @click="onPreviousGenerationClicked">Prev. Generation</AButton>
      <AButton @click="onNextGenerationClicked">Next Generation</AButton>
    </div>
  </div>
</template>

<script lang="ts">
import {defineComponent, Ref, ref} from 'vue';
import {MarsLanderApi} from "@/api/MarsLanderApi";
import {Map} from "@/views/mars-lander/models/environment/Map";
import * as Echarts from "echarts";
import AButton from "@/components/simple/AButton.vue";
import {Generation} from "@/views/mars-lander/models/evolution/Generation";

export default defineComponent({
  name: 'CMarsLander',
  components: {AButton},
  async setup() {
    const map = ref(null) as Ref<null | HTMLDivElement>;
    const selectedMap = ref('');

    const marsLanderApi = new MarsLanderApi();

    const graph = ref(null) as Ref;

    const mapElements = await marsLanderApi.GetMaps();

    const selectedGeneration = ref(0);

    function onMapChange() {
      selectedGeneration.value = 0;

      renderMap(true);
    }

    let generations: Generation[];

    function renderMap(reset = false) {
      if (graph.value === null) {
        graph.value = Echarts.init(map.value as HTMLDivElement);
      }
      if (reset)
        graph.value?.clear();
      const mapToRender = mapElements.find(element => element.Name === selectedMap.value) as Map;
      const surfaceArray = mapToRender.SurfaceElements.map(element => [element.X, element.Y]);
      const generationActors = generations?.find(g => g.GenerationNumber === selectedGeneration.value) as Generation | null;
      const chartSeries: any[] = [];
      chartSeries.push({ // Surface
        type: 'line',
        symbol: 'none',
        lineStyle: {
          color: '#8d0d0d',
          width: 2
        },
        data: surfaceArray
      });
      for (const actor of generationActors?.Actors ?? []) {
        chartSeries.push({ // Actors in generation
          type: 'line',
          lineStyle: {
            width: 1
          },
          symbol: 'arrow',
          symbolSize: 6,
          symbolRotate: (value, params) => {
            return params.data?.rotation ?? 0;
          },
          data: actor.Lander.Situations.map((situation, index) => {
            return {
              rotation: situation.Rotation,
              value: [situation.X, situation.Y],
              score: actor.Score,
              action: actor.Lander.Actions[index],
              fuel: situation.Fuel,
              power: situation.Power,
              horizontalSpeed: situation.HorizontalSpeed,
              verticalSpeed: situation.VerticalSpeed
            }
          }),
          tooltip: {
            formatter: (params) => {
              return `X: ${params.data.value[0]} Y: ${params.data.value[1]}<br/>
              Rotation: ${params.data.rotation} Rotation: ${params.data.rotation}<br/>
              Power: ${params.data.power} Fuel: ${params.data.fuel}<br/>
              HSpeed: ${params.data.horizontalSpeed} VSpeed: ${params.data.verticalSpeed}<br/>
              Action: ${params.data.action} Score: ${params.data.score}`;
            }
          }
        })
      }
      const chartOptions = {
        title: {
          text: mapToRender.Name
        },
        xAxis: {
          min: 0,
          max: 7000
        },
        yAxis: {
          min: 0,
          max: 3000
        },
        tooltip: {
          trigger: 'item'
        },
        series: chartSeries
      } as Echarts.EChartsOption;
      console.dir(chartOptions);
      graph.value.setOption(chartOptions);

    }

    async function requestLanding() {
      generations = await marsLanderApi.Land(mapElements.find(element => element.Name === selectedMap.value) as Map);
      selectedGeneration.value = 1;

      renderMap();
    }

    function onNextGenerationClicked() {
      selectedGeneration.value += 1;
      renderMap();
    }

    function onPreviousGenerationClicked() {
      selectedGeneration.value -= 1;
      renderMap();
    }

    return {
      mapElements,
      selectedMap,
      onMapChange,
      map,
      requestLanding,
      selectedGeneration,
      onNextGenerationClicked,
      onPreviousGenerationClicked
    };
  }
})
</script>

<style scoped lang="scss">
#map {
  width: 1200px;
  height: 720px;
}
</style>
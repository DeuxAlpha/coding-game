<template>
  <Suspense>
    <template #default>
    </template>
    <template #fallback>
      Loading
    </template>
  </Suspense>
</template>

<script lang="ts">
import {defineComponent} from 'vue';

export default defineComponent({
  name: 'GreatEscape'
})
</script>

<style scoped lang="scss">

</style>
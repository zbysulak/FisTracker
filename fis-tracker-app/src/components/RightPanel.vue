<template>
  <v-container>
    <v-row>
      <v-col class="mb-2">
        <h4 class="mb-4">Overview</h4>
        <p class="mb-1">
          <span class="subheading mr-1">Month: </span>
          <b>{{ time.totalTime.totalHours }}</b
          ><span> hours</span>
        </p>
        <p class="mb-1">
          <span class="subheading mr-1">Hours Left: </span>
          <b>{{ time.totalTime.totalHours - time.timeNeeded.totalHours }}</b>
        </p>
      </v-col>
    </v-row>
    <v-row> </v-row>
  </v-container>
</template>

<script>
export default {
  name: "RightPanel",
  props: ["time"],
  components: {},

  data: () => ({
    totalTime: null,
    timeLeft: null
  })
}
</script>

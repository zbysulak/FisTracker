<template>
  <div>
    <month-picker v-model="month" v-on:input="monthChange" />
    <v-row>
      <v-col cols="8">
        <v-data-table
          :headers="headers"
          :items="timeInputs"
          sort-by="calories"
          class="elevation-1"
          :hide-default-footer="true"
          :item-class="workDayStyle">
          <template v-slot:top>
            <v-toolbar flat>
              <v-spacer></v-spacer>
              <time-edit-dialog
                v-model="editedItem"
                v-on:saved="updateTable"></time-edit-dialog>
              <v-dialog v-model="dialogDelete" max-width="500px">
                <v-card>
                  <v-card-title class="text-h5">
                    Are you sure you want to delete this item?</v-card-title
                  >
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="blue darken-1" text @click="closeDelete"
                      >Cancel</v-btn
                    >
                    <v-btn color="blue darken-1" text @click="deleteItemConfirm"
                      >OK</v-btn
                    >
                    <v-spacer></v-spacer>
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </v-toolbar>
          </template>
          <template v-slot:[`item.date`]="{ item }">
            {{ item.date | formatDate }}
          </template>
          <template v-slot:[`item.homeOffice`]="{ item }">
            <v-icon v-if="item.homeOffice" class="text-center">
              mdi-checkbox-marked-circle-outline
            </v-icon>
          </template>
          <template v-slot:[`item.actions`]="{ item }">
            <v-icon small class="mr-2" @click="editItem(item)">
              mdi-pencil
            </v-icon>
            <v-icon small @click="deleteItem(item)"> mdi-delete </v-icon>
          </template>
          <template v-slot:no-data>
            <v-btn color="primary" @click="initialize"> Reset </v-btn>
          </template>
        </v-data-table>
      </v-col>
      <v-col cols="4">
        <right-panel></right-panel>
      </v-col>
      <v-col>
        <z-image-upload />
        <right-panel :time="time"></right-panel>
      </v-col>
    </v-row>
    <v-overlay :value="overlay">
      <v-progress-circular indeterminate size="64"></v-progress-circular>
    </v-overlay>
  </div>
</template>
<script>
import axios from "axios"
import RightPanel from "./RightPanel.vue"
import TimeEditDialog from "./TimeEditDialog.vue"
import ZImageUpload from "./ZImageUpload.vue"
import MonthPicker from "./MonthPicker.vue"

export default {
  components: { TimeEditDialog, ZImageUpload, RightPanel, MonthPicker },
  data: () => ({
    month: new Date().toISOString().substring(0, 7),
    dialog: false,
    dialogDelete: false,
    headers: [
      {
        text: "Date",
        value: "date"
      },
      { text: "Arrived at", value: "in.formatted" },
      { text: "Lunch break start", value: "lunchOut.formatted" },
      { text: "Lunch break end", value: "lunchIn.formatted" },
      { text: "Left at", value: "out.formatted" },
      { text: "Homeoffice / holiday", value: "homeOffice" },
      { text: "Actions", value: "actions", sortable: false }
    ],
    timeInputs: [],
    time: {},
    datePicker: false,
    editedIndex: -1,
    editedItem: {},
    defaultItem: {
      date: "",
      in: "08:00",
      lunchOut: null,
      lunchIn: null,
      out: null,
      homeOffice: false
    },
    date: "2021-11",
    overlay: false
  }),

  watch: {
    dialog(val) {
      val || this.close()
    },
    dialogDelete(val) {
      val || this.closeDelete()
    }
  },

  created() {
    this.initialize()
  },

  methods: {
    monthChange(a) {
      console.log("month changed to", a)
      this.loadTable(a)
    },
    updateTable() {
      this.loadTable(this.date)
    },
    loadTable(yearMonth) {
      console.log("huh?")
      const m = yearMonth.split("-")
      const year = m[0]
      const month = m[1]
      axios
        .get(
          this.appConfig.apiUrl +
            "/TimeInputs?month=" +
            month +
            "&year=" +
            year,
          {
            withCredentials: true
          }
        )
        .then((d) => {
          console.log(d)
          this.timeInputs = d.data.timeInputs
          this.time = d.data
        })
        .catch((d) => console.error(d))
    },
    workDayStyle(item) {
      return item.workDay ? "" : "grey lighten-3"
    },
    initialize() {
      this.loadTable(this.month)
    },
    deleteItem(item) {
      this.editedIndex = this.timeInputs.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialogDelete = true
    },
    deleteItemConfirm() {
      this.desserts.splice(this.editedIndex, 1)
      this.closeDelete()
    },
    editItem(item) {
      this.editedIndex = this.timeInputs.indexOf(item)
      this.editedItem = Object.assign({}, item)
      console.log(this.editedItem)
      this.dialog = true
    },
    closeDelete() {
      this.dialogDelete = false
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem)
        this.editedIndex = -1
      })
    }
  }
}
</script>

<template>
  <v-row>
    <v-row>
      <v-col>
        <v-menu
          ref="menu"
          v-model="menu"
          :close-on-content-click="false"
          :return-value.sync="date"
          transition="scale-transition"
          offset-y
          max-width="290px"
          min-width="auto">
          <template v-slot:activator="{ on, attrs }">
            <v-text-field
              v-model="date"
              label="Select month"
              prepend-icon="mdi-calendar"
              readonly
              v-bind="attrs"
              v-on="on"></v-text-field>
          </template>
          <v-date-picker v-model="date" type="month" no-title scrollable>
            <v-spacer></v-spacer>
            <v-btn text color="primary" @click="menu = false"> Cancel </v-btn>
            <v-btn text color="primary" @click="$refs.menu.save(date)">
              OK
            </v-btn>
          </v-date-picker>
        </v-menu>
      </v-col>
    </v-row>
    <v-row>
      <v-col>
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
            <v-chip
              v-if="item.homeOffice"
              class="ma-2"
              color="green"
              text-color="white">
              JO!
            </v-chip>
            <v-chip v-else class="ma-2" color="orange" text-color="white">
              NE :(
            </v-chip>
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
      <v-col>
        <z-image-upload />
        <right-panel></right-panel>
      </v-col>
    </v-row>
  </v-row>
</template>
<script>
import axios from "axios"
import RightPanel from "./RightPanel.vue"
import TimeEditDialog from "./TimeEditDialog.vue"
import ZImageUpload from "./ZImageUpload.vue"
export default {
  components: { TimeEditDialog, ZImageUpload, RightPanel },
  data: () => ({
    test: "ahoj",
    time: "8:00",
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
    menu: false
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
    updateTable() {
      console.log("updateTable")
    },
    workDayStyle(item) {
      return item.workDay ? "" : "grey lighten-3"
    },
    initialize() {
      axios
        .get(
          this.appConfig.apiUrl + "/TimeInputs?month=" + 11 + "&year=" + 2021,
          { withCredentials: true }
        )
        .then((d) => {
          console.log(d)
          this.timeInputs = d.data.timeInputs
        })
        .catch((d) => console.error(d))
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
